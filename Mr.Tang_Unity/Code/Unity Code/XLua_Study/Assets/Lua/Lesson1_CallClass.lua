print("**********Lua调用C#类相关知识点**********")


-- lua中使用C#的类非常简单
-- 固定查路：CS.命名空间.类名
-- Unity的类 比如 GameObject、Transform 等等 —— CS.UnityEngine.GameObject、CS.UnityEngine.Transform

-- 通过C#中的类 实例化一个对象 lua中没有new 所以直接类名加括号 就是实例化对象
local obj1 = CS.UnityEngine.GameObject()
local obj2 = CS.UnityEngine.GameObject("小凌哥") -- 传入名字参数

-- 但是每次都这样写CS.UnityEngine. 是不是太长了 而且每次.在性能上也会有损耗 Lua 会先查找全局表 CS，再查找其子表 UnityEngine，最后查找 GameObject，每一步都是一次哈希表查找
-- 所以为了方便使用 节约性能 可以定义一个局部变量 相当于取了一个别名
GameObject = CS.UnityEngine.GameObject -- 这个GameObject 只在开始第一次赋值时 会有查找CS.UnityEngine.GameObject的过程
-- 可以理解成需要查找三次地址 CS -> UnityEngine -> GameObject , 但是赋值给局部变量GameObject后 直接记住了最终GameObject的地址 以后使用GameObject就不需要再查找前面的了
local obj3 = GameObject("阿伟") -- 直接用别名来实例化对象

-- 类中的静态对象 可以直接通过类名.静态成员 来访问
local obj4 = GameObject.Find("小凌哥") -- 静态方法

-- 得到对象中的成员变量 直接用点语法
print(obj4.transform.position) -- transform是GameObject的成员变量
Debug = CS.UnityEngine.Debug -- 给Debug也取个别名
Debug.Log(obj4.transform.position) -- 也可以通过C#的Debug类来打印

-- 如果使用对象中的成员方法！！ 一定要加 冒号:   冒号的特性还记得吗？ 语法糖 它会把调用者作为第一个参数传递进去
Vector3 = CS.UnityEngine.Vector3 -- 给Vector3取个别名
obj4.transform:Translate(Vector3.right) -- 把小凌哥往右移动
Debug.Log(obj4.transform.position)

-- 到此我们可以看到 Lua调用C#类 是非常简单的 当然 上述用的都是U3D的类 如果是自定义的C#类 该如何调用呢？
-- 我们在Unity中写了一个LuaCallCSharp脚本 里面有一个Test类没有在任何namespace中 和一个Test2类在 MrLing命名空间下 两个类中各有个Speak方法
-- 我们可以在lua中这样调用
local t = CS.Test() -- 实例化Test类
t:Speak("你好啊！没有namespace的小凌哥") -- 调用Speak方法
local t2 = CS.MrLing.Test2() -- 实例化MrLing命名空间下的Test2类
t2:Speak("你好啊！MrLing下的小凌哥") -- 调用Speak方法

-- 好，那继承了MonoBehaviour的类呢？ 继承了MonoBehaviour的类是不能直接new的
local obj5 = CS.UnityEngine.GameObject("加脚本测试") -- 先创建一个空物体
-- 然后通过AddComponent来添加脚本组件
local luaCallCSharp = obj5:AddComponent(typeof(CS.LuaCallCSharp)) -- 我们之前谈到过lua不支持泛型 所以这里的typeof是Xlua提供的一个替代方法 用来获取类型
-- 这样就成功添加了LuaCallCSharp脚本组件
-- 然后我们就可以调用LuaCallCSharp脚本中的方法了
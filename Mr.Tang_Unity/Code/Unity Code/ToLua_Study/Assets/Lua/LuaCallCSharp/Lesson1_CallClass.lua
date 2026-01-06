print("------------------ ToLua访问CSharp的类 ------------------")


--toLua和xlua访问C#类非常类似
--固定写法：
--命名空间.类名
--Unity的类 比如 GameObject Transform等  ——> UnityEngine.类名
--UnityEngine.GameObject

--通过C#中的类实例化一个对象 Lua中没有new 所以我们直接使用 类名括号就是实例化对象
--默认调用的 相当于是无参构造

-- 和xlua的区别是 不需要加CS.命名空间.类名 省略了CS
local obj1 = UnityEngine.GameObject()  --实例化了一个GameObject对象 
-- 这里有个关键知识点 必选在ToLuaManager中加上LuaBinder.Bind(luaState);这句代码 把ToLua生成的绑定类 绑定到解析器中 才能使用Unity的类

local obj2 = UnityEngine.GameObject("小凌哥") --实例化一个名字叫"小凌哥"的GameObject对象

-- 为了方便使用并且节约性能 定义全局变量来储存我们C#中的类
-- 相当于取别名
GameObject = UnityEngine.GameObject
local obj3 = GameObject("DavidLing") --实例化一个名字叫"DavidLing"的GameObject对象

-- 类中的静态对象 可以使用.来访问
local obj4 = GameObject.Find("小凌哥") --查找场景中的对象
-- 得到对象中的成员变量 也是直接对象.属性 即可
print(obj4.transform.position.x)

Debug = UnityEngine.Debug -- 给Debug取个别名
-- 初次测试报错，为什么呢？ 因为我们没有把它加到CustomSettings自定义类型里并生成代码
Debug.Log("使用ToLua访问C#类的Debug方法") -- 现在可以正常使用了
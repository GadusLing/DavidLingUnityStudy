print("------------------ ToLua访问CSharp的重载方法------------------")

local obj = Lesson6()

print(obj:Calc())
-- toLua 和Xlua一样 对重载函数的精度支持不好
-- lua中只有number类型 是一个double
print(obj:Calc(1))
print(obj:Calc(1.2))
print(obj:Calc("123"))

print(obj:Calc(10, 1))
-- 那如果我就是要用out参数怎么办呢？
print(obj:Calc(10, nil)) -- 固定写法 传nil表示不需要传入第二个参数 它是个out参数
-- 这种写法有什么用？unity有些API是有out参数的 比如射线检测的hitInfo参数
-- 例：Physics.Raycast(ray, out hitInfo)
-- local result, hitInfo = Physics.Raycast(ray, nil) -- 这种写法就可以拿到hitInfo参数

-- 这里我们再研究一下为什么tolua的ref不好用
-- 我们在LuaCallCSharp.cs中把 public int Calc(int a, out int b) 改成 public int Calc(int a, ref int b)
-- 然后发现 print(obj:Calc(10, nil)) 报错了 这是因为 ref参数在C#中调用时 必须传入一个初始化的变量
-- 所以我们改成 print(obj:Calc(10, 5))，这个时候问题就出现了，lua重载无法识别ref
-- 所以传进来的5 会被当成一个普通的int参数 结果调用了 public int Calc(int a, int b) 这个重载方法
-- 所以在tolua中 可以忽略掉ref参数的重载方法 只用out参数的重载方法
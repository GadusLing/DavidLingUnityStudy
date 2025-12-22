print("*********Lua调用C# 拓展方法 相关知识点***********")

Lesson4 = CS.Lesson4
-- 使用静态方法
-- CS.命名空间.类名.静态方法名(参数)
Lesson4.Eat()

-- 成员方法 实例化出来使用
local obj = Lesson4() -- 实例化
obj:Speak("hahahahahah") -- 调用成员方法

-- 拓展方法和使用成员方法一样  -- 要调用C#中某个类的拓展方法 一定要加上[XLua.LuaCallCSharp]特性 并且去Unity 菜单栏点击 XLua/Generate Code 生成代码
obj:Move()

-- 总结
-- Lua 可以直接调用 C# 的扩展方法
-- 但前提是：必须在扩展方法对应的静态类前加上 [XLua.LuaCallCSharp] 特性，并在 Unity 菜单栏点击 XLua/Generate Code 生成代码
-- 这样 Lua 才能访问到扩展方法，否则会报 nil 错误

-- 建议：只要是 Lua 侧需要访问的 C# 类，都加上 [XLua.LuaCallCSharp] 特性
-- 这样不仅能访问扩展方法，还能提升 Lua 访问 C# 的性能
-- 这里还遗留一个问题，我的自定义类可以加这个特性，但是系统自带的类比如 GameObject、Transform 这些类我没办法加这个特性
-- 这个问题留到后面Lua和系统类交互时再说
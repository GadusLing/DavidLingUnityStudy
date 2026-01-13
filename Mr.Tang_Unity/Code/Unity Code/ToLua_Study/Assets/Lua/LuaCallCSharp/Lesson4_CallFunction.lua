print("------------------ ToLua访问CSharp的拓展方法 ------------------")

--静态方法的调用 就是类名.方法名
Lesson4.Eat()

--成员方法
local obj = Lesson4()
obj:Speak("Hello from Lua") --成员方法调用用冒号

-- 想要拓展方法使用成功，Xlua里需要在类之前加个LuaCallCSharp的特性
-- tolua没有这个特性 而是要在CustomSettings的extendTypeList里添加的类后面.AddExtendType(typeof(拓展类)) 并再生成一次代码
-- _GT(typeof(Lesson4)).AddExtendType(typeof(Tools)), // 给Lesson4类添加拓展方法
obj:Move()
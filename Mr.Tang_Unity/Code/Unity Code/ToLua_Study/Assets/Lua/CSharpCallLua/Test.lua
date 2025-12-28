print("CSharp调用Lua的测试脚本")

-- 全局变量
testNumber = 1
testBool = true
testFloat = 1.2
testString = "Hello Lua"

-- 无法通过C#获取本地local变量
local testLocal = 10

testFun = function()
    print("无参无返回函数")
end

testFun2 = function(a)
    print("有参有返回函数，参数a:", a)
    return a + 100
end

testFun3 = function(a)
    print("多返回值函数，参数:", a)
    return 1, 2, false, "Lua多返回值", a
end
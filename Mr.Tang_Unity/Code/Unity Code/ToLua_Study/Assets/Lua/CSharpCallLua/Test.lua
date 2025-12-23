print("CSharp调用Lua的测试脚本")

-- 全局变量
testNumber = 1
testBool = true
testFloat = 1.2
testString = "Hello Lua"

-- 无法通过C#获取本地local变量
local testLocal = 10
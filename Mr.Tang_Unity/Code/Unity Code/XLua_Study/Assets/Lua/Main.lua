print("主Lua脚本启动")

-- 判空全局函数
function IsNull(obj)
    if obj == nil or obj:Equals(nil) then
        return true
    end
    return false
end


-- Unity中写lua执行
-- xlua帮我们重定向了lua脚本的加载方式
-- 只要是执行lua脚本 都会调用我们自定义的Loader函数
-- 我们之前在XluaManager中写了 DoLuaFile -> DoString -> luaEnv.DoString(require('xxx')) 
-- 其实在这里就相当于直接写了  require('xxx')
-- 这个require函数会调用我们自定义的Loader函数 来加载lua脚本
-- require("Test") -- CSharpCallLua相关的测试脚本
-- require("Lesson1_CallClass") -- Lua调用CSharp相关的测试脚本 Lesson1
-- require("Lesson2_CallEnum") -- Lua调用CSharp相关的测试脚本 Lesson2
-- require("Lesson3_CallArray") -- Lua调用CSharp相关的测试脚本 Lesson3
-- require("Lesson4_CallFunction") -- Lua调用CSharp相关的测试脚本 Lesson4
-- require("Lesson5_CallFunction") -- Lua调用CSharp相关的测试脚本 Lesson5·
-- require("Lesson6_CallFunction") -- Lua调用CSharp相关的测试脚本 Lesson6
-- require("Lesson7_CallDel") -- Lua调用CSharp相关的测试脚本 Lesson7
-- require("Lesson8_Special") -- Lua调用CSharp相关的测试脚本 Lesson8
-- require("Lesson9_Special") -- Lua调用CSharp相关的测试脚本 Lesson9
-- require("Lesson9_Special") -- Lua调用CSharp相关的测试脚本 Lesson9
-- require("Lesson10_Special") -- Lua调用CSharp相关的测试脚本 Lesson10
-- require("Lesson11_Coroutine") -- Lua调用CSharp相关的测试脚本 Lesson11
-- require("Lesson12_T") -- Lua调用CSharp相关的测试脚本 Lesson12
-- require("HotFix_Lesson1") -- HotFix相关的测试脚本 Lesson1
-- require("HotFix_Lesson2") -- HotFix相关的测试脚本 Lesson2
-- require("HotFix_Lesson3") -- HotFix相关的测试脚本 Lesson3
-- require("HotFix_Lesson4") -- HotFix相关的测试脚本 Lesson4
-- require("HotFix_Lesson5") -- HotFix相关的测试脚本 Lesson5
require("HotFix_Lesson6") -- HotFix相关的测试脚本 Lesson6

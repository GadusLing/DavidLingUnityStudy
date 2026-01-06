print("CSharp调用Lua的测试脚本")

-- 全局变量
testNumber = 1
testBool = true
testFloat = 1.2
testString = "Hello Lua"

-- 无法通过C#获取本地local变量
local testLocal = 10

-- 无参无返回函数
testFun = function()
    print("无参无返回函数")
end

-- 有参有返回函数
testFun2 = function(a)
    print("有参有返回函数，参数a:", a)
    return a + 100
end

-- 多返回值函数
testFun3 = function(a)
    print("多返回值函数，参数:", a)
    return 1, 2, false, "Lua多返回值", a
end

-- 变长参数函数
testFun4 = function(a, ...)
    print("变长参数函数")
    print("参数a:", a)
    arg = {...}
    for k, v in pairs(arg) do
        print(k, v)
    end
end

-- table表现List
testList = {1, 2, 3, 4, 5, 6}
testList2 = {"Lua", "CSharp", true, false, 3.14}

-- table表现Dictionary
testDic = {name = "Mr.Ling", age = 28, isMale = true}
testDic2 = {["Lua"] = "脚本语言", ["CSharp"] = "编程语言", ["Unity3D"] = "游戏引擎", [true] = true}

-- lua中的自定义Table
testClass = {testInt = 10, testString = "Hello TestClass", testFloat = 3.14,
    testFun = function()
        print("testClass中的testFun被调用")
    end
}

-- lua中的协程函数
local coDelay = nil

-- 这些协程写法都是tolua提供的扩展函数
-- lua本身不是这样开启协程的 是create resume yield status wrap running等函数
StartDelay = function ()
    coDelay = coroutine.start(Delay)
end

Delay = function ()
    local c = 1
    while true do
        coroutine.wait(1)
        print("Lua协程函数 Delay:" .. c)
        c = c + 1
        if c > 5 then
            StopDelay()
            break
        end
    end
end

StopDelay = function ()
    if coDelay ~= nil then
        coroutine.stop(coDelay)
        coDelay = nil
    end
end
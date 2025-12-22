-- 测试用的Lua变量定义
-- testNumber: 数值类型
-- testBool: 布尔类型
-- testFloat: 浮点数类型
-- testString: 字符串类型
testNumber = 1
testBool = true
testFloat = 1.2
testString = "123"
print("Test.lua脚本被加载执行了")

local testlocal = 10
-- 通过C#无法访问到这个局部变量 它的作用域仅限于Test.lua脚本内部

-- 测试用的Lua函数定义

-- 无参数无返回
testFun = function ()
    print("无参无返回函数被调用了")
end

-- 有参数有返回
testFun2 = function (a)
    print("有参有返回函数被调用了 参数a:", a)
    return a + 1
end

-- 多返回
testFun3 = function (a)
    print("多返回函数被调用了 参数a:", a)
    return a, 1, false, "hello"
end

-- 变长参数
testFun4 = function (...)
    local args = {...}
    print("变长参数函数被调用了 参数个数:", #args)
    for i, v in pairs(args) do
        print("参数", i, "的值为:", v)
    end
end

-- List
testList = {1, 2, 3, 4, 5, 6} -- 固定类型
testList2 = {"hello", 123, true, 4.56} -- 混合类型

-- Dictionary
testDic = {
    ["1"] = 1,
    ["2"] = 2,
    ["3"] = 3,
    ["4"] = 4,
}

testDic2 = {
    ["1"] = 1,
    [true] = 2,
    [false] = true,
    ["123"] = false
}

testClass = {
    testInt = 2,
    testBool = true,
    testFloat = 3.14,
    testString = "hello",
    testFun = function ()
        print("testClass中的testFun被调用了")
    end--,
    -- testInClass = {
    --     testInInt = 1,
    --     testInBool = 2
    -- } -- 尝试在表中嵌套表 测试在C#中能否正确访问
}
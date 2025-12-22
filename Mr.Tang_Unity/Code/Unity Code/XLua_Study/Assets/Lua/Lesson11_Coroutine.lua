print("*********Lua调用C# 协程 相关知识点***********")
-- 协程Coroutine 是Unity引擎中的一个重要概念
-- 协程可以让我们在多个帧之间分配任务，从而避免在单个帧中执行大量计算，导致游戏卡顿
-- 协程允许我们暂停函数的执行，并在后续的帧中继续执行，从而实现异步操作和延迟执行
-- C#种启动协程都是通过继承了Mono的类 里面的MonoBehaviour.StartCoroutine 方法来实现的


-- 因为不能直接将lua函数传给StartCoroutine 所以要用到xlua提供的一个工具表 一定要require之后才能用
util = require("xlua.util")

GameObject = CS.UnityEngine.GameObject
WaitForSeconds = CS.UnityEngine.WaitForSeconds
MonoBehaviour = CS.UnityEngine.MonoBehaviour

-- 在场景种创建一个空物体 并添加一个LuaCallCSharp脚本 脚本继承mono 用来启动协程
local obj = GameObject("Coroutine对象")
local mono = obj:AddComponent(typeof(CS.LuaCallCSharp))

fun = function ()
    local a = 1
    while true do
        -- Lua种不能直接使用C#种的 yield return 语法
        -- 要使用lua的协程相关API coroutine.yield 来实现协程的暂停和恢复
        print("函数fun执行 第"..a.."次")
        a = a + 1
        coroutine.yield(WaitForSeconds(1)) -- 每次执行后等待1秒钟
        if a > 5 then
            mono:StopCoroutine(b) -- 停止协程
            print("协程停止了")
        end
    end
end
    
-- 初次尝试发现这样写会报错 不能直接将lua函数传给StartCoroutine
--mono:StartCoroutine(fun) -- 尝试用C#的写法启动协程 失败

-- 需要用xlua提供的util.cs_generator函数
b = mono:StartCoroutine(util.cs_generator(fun)) -- 成功启动协程
-- 关的话就先存起来 然后用mono:StopCoroutine(b) 停止协程

-- 总结：
    -- 1. Lua 中调用 C# 协程的开启和关闭规则与 C# 基本一致 就是使用方面有所不同。
    -- 2. C# 的 yield return 相当于 Lua 中的 coroutine.yield(返回值)。
    -- 3. 不能直接把 Lua 函数传给 C# 的 StartCoroutine，必须用 xlua.util.cs_generator(函数) 包装后再传入。
    -- 4. 启动协程前，util 表必须先 require("xlua.util") 才能用。
    -- 5. C# 协程的启动都是通过继承了 MonoBehaviour 的类的 StartCoroutine 方法来实现的。
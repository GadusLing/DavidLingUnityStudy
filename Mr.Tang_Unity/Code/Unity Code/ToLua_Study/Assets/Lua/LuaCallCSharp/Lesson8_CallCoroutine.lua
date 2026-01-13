print("------------------ ToLua访问CSharp的协程------------------")

--tolua提供给我们了一些方便开启协程的方法

local coDelay = nil
StartDelay = function()
    --tolua提供给我们开启协程的方法
    coDelay = StartCoroutine(Delay) -- 想起来没 这是unity的API tolua也用同名封装了
end

Delay = function()
    local c = 1
    while true do
        WaitForSeconds(1)  -- 等待1秒 写法和Unity中API一样 实际上都是tolua封装的同名函数
        -- Yield(0) -- 等待一帧 也和Unity中API一样
        -- WaitForFixedUpdate() -- 等待FixedUpdate物理更新阶段后执行
        -- WaitForEndOfFrame() -- 等待一帧渲染完成执行
        -- Yield(异步加载返回值) -- 等待异步加载完成后执行
        print("协程执行了"..c.."次")
        c = c + 1

        if c > 5 then
            StopDelay()
            break
        end
    end
end

StopDelay = function()
    -- 停止协程
    StopCoroutine(coDelay) -- 和Unity中API一样
    coDelay = nil
    print("协程被停止了")
end

StartDelay()



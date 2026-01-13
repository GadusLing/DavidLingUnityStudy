print("------------------ ToLua访问CSharp的协程------------------")

--tolua提供给我们了一些方便开启协程的方法

StartDelay = function()
    --tolua提供给我们开启协程的方法
    StartCoroutine() -- 想起来没 这是unity的API tolua也用同名封装了
end

Delay = function()
    local c = 1
    while true do
        WaitForSeconds(1)  -- 等待1秒 写法和Unity中API一样
        -- Yield(0) -- 等待一帧 也和Unity中API一样
        -- WaitForFixedUpdate() -- 等待FixedUpdate阶段
        -- WaitForEndOfFrame() -- 等待渲染完成
        -- Yield(异步加载返回值)
    end
end



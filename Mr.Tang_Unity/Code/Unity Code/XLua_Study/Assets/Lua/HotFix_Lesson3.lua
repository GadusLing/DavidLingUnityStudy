print("**********协程函数替换**********")

-- 因为不能直接将lua函数传给StartCoroutine 所以要用到xlua提供的一个工具表 一定要require之后才能用
util = require("xlua.util")

xlua.hotfix(CS.HotFixMain, {
    TestCoroutine = function(self)
        return util.cs_generator(function()
            while true do
                coroutine.yield(CS.UnityEngine.WaitForSeconds(1))  -- 注意这里要用coroutine.yield()而不是yield return null
                print("这是被HotFix替换后的协程函数")
            end
        end)
    end
})

-- 如果我们为打了Hotfix特性的C#类新加了函数内容
-- 不能只注入，必须要先生成代码，再注入，否则注入会报错
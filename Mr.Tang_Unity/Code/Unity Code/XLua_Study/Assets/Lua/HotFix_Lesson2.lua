print("**********多函数替换**********")

-- 多函数替换的固定写法 xlua.hotfix(类, { 函数名 = lua函数, 函数名 = lua函数 })
xlua.hotfix(CS.HotFixMain, {
    Update = function(self)
        print(os.time())
    end,
    add = function(self, a, b)
        return a * b
    end,
    Speak = function(str)
        print("这是Lua热补丁后的Speak方法" .. str)
    end
})

xlua.hotfix(CS.HotfixTest, {
    [".ctor"] = function(self)
        print("构造函数热补丁固定写法  [\".ctor\"]")
    end,
    Speak = function(self, str)
        print("HotfixTest类的Speak方法被热补丁替换了 " .. str)
    end,
    Finalize = function(self)
        print("析构函数热补丁固定写法 Finalize")
    end
    -- 注意 热补丁的构造和析构函数都不是替换 而是先调用原函数 然后调用lua热补丁函数
})
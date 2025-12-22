print("**********属性 和 索引器的替换**********")

xlua.hotfix(CS.HotFixMain, {
    -- 属性热补丁的固定写法
    -- set_属性名 = function(self, value) end
    -- get_属性名 = function(self) return value end
    set_Age = function(self, value)
        print("这是被HotFix替换后的set_Age函数，参数value=" .. value)
    end,
    get_Age = function(self)
        print("这是被HotFix替换后的get_Age函数")
        return 100  -- 替换后直接返回100
    end,

    --索引器热补丁的固定写法
    -- set_Item = function(self, index, value) end
    -- get_Item = function(self, index) return value end
    set_Item = function(self, index, value)
        print("这是被HotFix替换后的set_Item函数，参数index=" .. index .. ", value=" .. value)
    end,
    get_Item = function(self, index)
        print("这是被HotFix替换后的get_Item函数，参数index=" .. index)
        return 999  -- 替换后直接返回999
    end,
})
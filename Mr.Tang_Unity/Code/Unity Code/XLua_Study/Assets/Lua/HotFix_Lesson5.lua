
print("**********事件加减替换**********")

xlua.hotfix(CS.HotFixMain, {
    -- 事件加减的写法
    -- add_EventName = function(self, handler)
    -- remove_EventName = function(self, handler)
    --add_事件名 代表着加操作
    --remove_事件名  减操作
    add_myEvent = function(self, del)
        print(del)
        print("添加事件函数")
        --会尝试通过lua使用C#事件的加方法去添加
        --在事件加减的重定向到lua函数中
        --千万不要把传入的委托往事件里存
        --否则会死循环
        --会把传入的 函数 存在lua中！！！！
        --self:myEvent("+", del)
    end,
    remove_myEvent = function(self, del )
        print(del)
        print("移除事件函数")
    end
})

-- 小结：
    -- 1. 事件加减重定向到Lua时，add_事件名/remove_事件名是固定写法。
    -- 2. 如果要重定向事件加减到Lua，一定不要再用C#的事件加减（如 self:myEvent("+", del)），否则会造成死循环！
    -- 3. 正确做法是把传入的函数存到Lua的容器中，在lua中创建一张表来组织函数 由Lua自行管理和处理。
    -- 4. 这样才能让事件逻辑完全交给Lua侧处理，避免逻辑混乱。


print("*********Lua调用C# nil和null比较 相关知识点***********")

-- 往场景对象上添加一个脚本 如果存在就不添加 如果不存在就添加
GameObject = CS.UnityEngine.GameObject
Rigidbody = CS.UnityEngine.Rigidbody
local obj = GameObject("测试添加脚本")
local rig = obj:GetComponent(typeof(Rigidbody))
print(rig)
-- if rig:Equals(nil) then -- 第一种方法
-- if IsNull(rig) then -- 第二种方法
if rig:IsNull() then -- 第三种方法
    print("Rigidbody组件不存在，添加Rigidbody组件") -- 发现这一行没有进来 是为何？ nil和null不能进行==比较 需要用 Equals方法比较
    rig = obj:AddComponent(typeof(Rigidbody))
end
print(rig)
-- 但是equals方法使用起来不方便 使用前提必须是 对象是C#的object类型 如果调用对象是lua的nil类型 不能调用 nil:Equals(...)
-- 所以我们可以为XLua写一个全局函数 IsNull 来判断C#对象是否为null

-- 总结 ：
        -- Lua 中的 nil 和 C# 的 null 不能直接用 == 比较，不是等价的
        -- 所以需要用三种方法来判断 C# 对象是否为 null

        -- 方法一：对象:Equals(nil)
        --   这种方法前提是对象是一个 C# object，且不能是 Lua 的 nil，否则会报错

        -- 方法二：在 Lua 中封装一个全局函数 IsNull(obj) 做保险判断
        --   function IsNull(obj)
        --       if obj == nil or obj:Equals(nil) then
        --           return true
        --       end
        --       return false
        --   end

        -- 方法三：在 C# 中为 Object 写一个扩展方法用于判空
        --   [LuaCallCSharp]
        --   public static class Lesson9
        --   {
        --       public static bool IsNull(this Object obj)
        --       {
        --           return obj == null;
        --       }
        --   }

        -- 推荐用方法二或三，能更安全地判断 C# 对象是否为 null

print("------------------ ToLua访问CSharp的数组 ------------------")

local obj = Lesson3()

--数组长度
print("数组长度 =", obj.array.Length) -- 这里不能用Lua的#操作符获取数组长度
-- 因为 # 只适用于 Lua 的 table 类型。C# 的数组在 Lua 里通常需要用 .Length 属性来获取长度

-- 访问数组元素
print("数组第1个元素 =", obj.array[0]) -- 注意：C#数组是从0开始索引的 不是lua的1开始索引 因为本质上是通过lua调C#的，所以保留了C#的索引规则

-- 查找元素
print("查找元素位置: ", obj.array:IndexOf(3)) -- 注意：C#数组的方法调用 需要用 : 语法糖 而不是 . 语法糖

-- 遍历数组 lua中遍历是从1开始的 但是数组是C#结构 lua for循环的结束点是<= 不是< 所以这里用 obj.array.Length - 1
for i = 0, obj.array.Length - 1 do
    print("遍历数组元素:", obj.array[i])
end

--tolua中比Xlua多了几种遍历方式















print("------------------ ToLua访问CSharp的List和Dictionary ------------------")

local obj = Lesson3()
print("-----访问CSharp的List----")
obj.list:Add(66) -- 成员方法一定是冒号调用
obj.list:Add(77)

print(obj.list[0]) -- 虽然自定义Lesson3已经注册过，但我们在里面新增了List和Dictionary类型的成员变量 所以要重新Generate一次

--得到长度
print("List长度:", obj.list.Count)

--遍历
for i = 0, obj.list.Count - 1 do
    print("List元素:", obj.list[i])
end

--在tolua中创建一个List
print("-----在Lua中创建List----")
--tolua对泛型支持比较糟糕 如果要使用泛型类型 需要先在CustomSettings.cs中注册
--List<string>
local list2 = System.Collections.Generic.List_string()
list2:Add("Lua")
list2:Add("CSharp")
list2:Add("ToLua")
for i = 0, list2.Count - 1 do
    print("List2元素:", list2[i])
end

--Dictionary
print("-----访问CSharp的Dictionary----")
obj.dic:Add(4, "four")
obj.dic:Add(5, "five")
print("Dictionary长度:", obj.dic.Count)
print("Dictionary元素key=4:", obj.dic[4])
--遍历
--还记得Xlua怎么遍历Dictionary吗？通过pairs，但是tolua不支持pairs遍历Dictionary
--只能通过迭代器来遍历
local iter = obj.dic:GetEnumerator()
while iter:MoveNext() do
    local kv = iter.Current
    print("Dictionary元素key=" .. kv.Key .. ", value=" .. kv.Value)
end


print("-----只遍历Dictionary的Key----")
local keyIter = obj.dic.Keys:GetEnumerator()
while keyIter:MoveNext() do
    local key = keyIter.Current
    print("Dictionary Key:" .. key)
end

print("-----只遍历Dictionary的Value----")
local valueIter = obj.dic.Values:GetEnumerator()
while valueIter:MoveNext() do
    local value = valueIter.Current
    print("Dictionary Value:" .. value)
end

-- 如果要在Lua中创建Dictionary 和List一样 需要先在CustomSettings.cs中注册
print("-----在Lua中创建Dictionary----")
local dic2 = System.Collections.Generic.Dictionary_int_string()

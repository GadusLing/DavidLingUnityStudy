print("*********Lua调用C# 数组 list dictionary相关知识点***********")

local obj = CS.Lesson3()

--lua 使用 C# 数组相关知识
print(obj.array.Length)  --获取数组长度 这里不能采用#obj.array获取长度 因为实际上这个数组传过来是一个userdata类型的 并不是lua的table类型 它会保留原语言的结构 所以要用原语言的方式获取长度

--访问元素
print(obj.array[0])

--遍历数组 要注意 虽然lua中索引从1开始 但这是C#数组 要符合原语言的规则 索引要从0开始 另外array.Length也要-1 因为lua的for参数是 for 开始, 结束 do 是包含结束值的
for i=0, obj.array.Length -1 do
    print("array element "..i.." is "..obj.array[i])
end

--在Lua中创建一个C#数组
local array2 = CS.System.Array.CreateInstance(typeof(CS.System.Int32), 10) --创建一个长度为10的int类型数组
print(array2.Length)
print(array2[0]) --默认值是0
print(array2[1]) --默认值是0
print(array2) -- 看看类型

print("*********Lua调用C# 数组 list相关知识点***********")
-- 调用成员方法要用冒号 :！！！！！！  
-- 聊深一点 为要用冒号 用.会报错 因为C#那边 比如Add这种函数，有一个隐藏的第一参数是this指针，表示调用这个函数的对象本身
-- 我们平时在C#中调用成员函数时，比如list.Add(1) 这个this指针会被编译器自动传递过去 表示对本身list对象调用Add函数，增加一个元素1
-- 但是在lua中 如果用.来调用成员函数 那么就意味着没有传递一个对象作为第一个参数过去 这样C#那边就会报错 因为它找不到这个函数的调用对象是谁
-- 所以在lua中调用成员函数时 一定要用: 这样lua会自动把调用这个函数的对象作为第一个参数传递过去 给C#那边当作this指针
-- 所以还记不记之前我们写lua时怎么等价的 list:Add(1) 其实等价于  list.Add(list, 1) 只是用:冒号的话 lua帮我们自动传递了第一个参数过去

obj.list.Add(obj.list, 1) -- 你看这样显式的写是不是也不报错
obj.list:Add(2)
obj.list:Add(3)
print("list count is "..obj.list.Count)
for i=0, obj.list.Count -1 do
    print("list element "..i.." is "..obj.list[i])
end
print(obj.list) -- 看看类型

-- 在lua中创建一个C#的List
-- 老版本
local list2 = CS.System.Collections.Generic["List`1[System.String]"]() -- 创建一个string类型的List 太麻烦 了解下算了
print(list2)
list2:Add("hello")
print(list2[0])
-- 新版本 Xlua > v2.1.12才能使用
local list_String = CS.System.Collections.Generic.List(CS.System.String) -- 注意 这里不是直接实例化 而是先用泛型类型构造出一个List<String>类型 相当于得到一个别名 再去实例化
local list3 = list_String() -- 这才是实例化
list3:Add("world")
print(list3[0])


print("*********Lua调用C# 数组 dictionary相关知识点***********")
obj.dict:Add(1,"one")
print(obj.dict[1])
--遍历和C#上有点差别 C#上是foreach (var kvp in dict) 这种形式
for k,v in pairs(obj.dict) do
    print("dict key "..k.." value "..v)
end

--在lua中创建一个C#的Dictionary
-- 老版本 要写一堆，不学了  感兴趣去官网看看 现在都用新的了

--新版本
local Dic_String_Vector3 = CS.System.Collections.Generic.Dictionary(CS.System.String, CS.UnityEngine.Vector3) -- 构造出 Dictionary<string, Vector3>类型 相当于得到一个别名
local dict2 = Dic_String_Vector3() -- 实例化
dict2:Add("point", CS.UnityEngine.Vector3.right)
for k,v in pairs(dict2) do
    print(k, v)
end

print(dict2["point"]) -- 这里为啥打印的是nil？ 在lua中创建的字典 通过键中括号的方式得不到 会是nil
-- 要通过get_Item方法来获取值
print(dict2:get_Item("point")) -- 通过get_Item方法才能得到值 这个和C#的字典索引器原理是一样的 索引器其实就是调用的get_Item方法 只是语法糖而已
-- 同理 设置值也要用set_Item方法
dict2:set_Item("point", CS.UnityEngine.Vector3.up)
print(dict2:get_Item("point"))

-- 还有没有别的方法去得呢？
-- Trygetvalue 方法
print(dict2:TryGetValue("point")) -- 返回两个值 第一个是bool表示是否找到 第二个是值本身

-- 总结：
-- 数组、List 和 Dictionary 在 Lua 中的使用都要遵循 C# 的规则

-- 数组创建方式：
-- CS.System.Array.CreateInstance(数组类型, 长度)

-- List 创建方式：
-- 老版本：CS.System.Collections.Generic["List`1[System.String]"]()
-- 新版本：
-- local List_String = CS.System.Collections.Generic.List(CS.System.String)
-- local list3 = List_String()

-- Dictionary 创建方式（新版本推荐）：
-- local Dic_String_Vector3 = CS.System.Collections.Generic.Dictionary(CS.System.String, CS.UnityEngine.Vector3)
-- local dict2 = Dic_String_Vector3()

-- Dictionary 自己创建时的特殊用法：
-- 取值用 get_Item(键)
-- 赋值用 set_Item(键, 值)

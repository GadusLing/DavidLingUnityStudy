print("*********Lua调用C# 二维数组 相关知识点***********")

local obj = CS.Lesson8()

-- 获取长度
print("二维数组 行数:" .. obj.array:GetLength(0))
print("二维数组 列数:" .. obj.array:GetLength(1))

-- 获取元素
-- print(obj.array[1,2]) -- 注意，在C#里是这样使用的 但是在Lua里不支持这种方式访问二维数组元素
-- print(obj.array[0][1]) -- 这样也是不行的
-- 正确的访问方式
print(obj.array:GetValue(1,2)) -- 获取第2行 第3列元素 6
print(obj.array:GetValue(0,1)) -- 获取第1行 第2列元素 2

for i=0, obj.array:GetLength(0)-1 do
    for j=0, obj.array:GetLength(1)-1 do
        print(string.format("array[%d,%d]=%d", i, j, obj.array:GetValue(i,j)))
    end
end
print("*********Lua调用C# ref和out方法 相关知识点***********")

Lesson5 = CS.Lesson5

local obj = Lesson5() -- 实例化

-- ref参数 会以多返回值的形式返回给Lua
-- 如果函数存在返回值 那第一个返回值是函数的返回值 后面的才是ref参数的值
-- 调用函数时含有ref参数 需要传入一个默认值 占位置 下面的例子 中间两个是ref 头尾不是ref 中间传了0和0作为占位符
-- a相当于函数的返回值
-- b 第一个ref
-- c 第二个ref
local a,b,c = obj:RefFun(1, 0, 0, 1)
print("RefFun函数返回值:", a)
print("RefFun函数 ref参数 b:", b)
print("RefFun函数 ref参数 c:", c)

-- out参数 也是以多返回值的形式返回给Lua
-- 如果函数存在返回值 那第一个返回值是函数的返回值 后面的才是out参数的值
-- out参数不需要传入默认值 占位置 直接正常传参
-- a相当于函数的返回值
-- b 第一个out
-- c 第二个out
local a,b,c = obj:OutFun(20, 30)
print("OutFun函数返回值:", a)
print("OutFun函数 out参数 b:", b)
print("OutFun函数 out参数 c:", c)

--ref和out参数的混合使用
-- 综合上述规则 ref需占位 out不用传 第一个返回值是函数返回值 后面依次是ref和out参数的值
local a,b,c = obj:RefOutFun(20, 1)
print("RefOutFun函数返回值:", a)
print("RefOutFun函数 ref参数 b:", b)
print("RefOutFun函数 out参数 c:", c)




-- ref和out参数总结
-- ref 和 out 的使用非常相似
-- 如果 Lua 调用 C# 中带有 ref 和 out 的函数，返回值会以多返回值的形式返回给 Lua
-- 传参时：ref 参数需要传默认值占位，out 参数可以省略不传
-- 返回时：如果 C# 函数有返回值，第一个返回值就是函数的返回值
--        后面的返回值依次对应 ref 和 out 参数（从左到右顺序）
--        这些返回值就是对应参数在函数内部被赋的新值


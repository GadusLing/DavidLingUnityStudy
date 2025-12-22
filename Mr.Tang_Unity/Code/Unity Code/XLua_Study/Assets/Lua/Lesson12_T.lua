print("*********Lua调用C# 泛型函数 相关知识点***********")

local obj = CS.Lesson12()

local child = CS.Lesson12.TestChild() -- 注意 内部类用.号连接
local father = CS.Lesson12.TestFather()

obj:TestFun1(child, father)
obj:TestFun1(father, child)
-- lua支持有约束有参数的泛型函数

--obj:TestFun2(child)
-- lua不支持无约束的泛型函数调用

--obj:TestFun3()
-- lua也不支持有约束无参数的泛型函数调用

--obj:TestFun4(child)
-- lua不支持非class的约束

-- 补充知识 让上面不支持的泛型函数支持
-- 得到通用函数 然后设置泛型类型 最后使用
-- xlua.get_generic_method(类, "函数名")
local testFun2 = xlua.get_generic_method(CS.Lesson12, "TestFun2")
local testFun2_R = testFun2(CS.System.Int32)
-- 调用
-- 成员方法 第一个参数传 调用函数的对象
-- 静态方法 不用传
testFun2_R(obj, 1)

-- 但是这种方法了解一下就算了 支持不是很全面 
-- Mono打包 这种方式是支持的
-- il2cpp打包 如果泛型参数是引用类型才可以使用
-- il2cpp打包 如果泛型参数是值类型 除非C#代码中已经调用过了 同类型的泛型参数 lua中才能够被使用

-- 总结：
    -- 1. Lua 调用 C# 泛型函数时，默认只支持“有约束且有参数”的泛型函数。
    -- 2. 无约束泛型函数、无参数泛型函数、非 class 约束的泛型函数，Lua 侧默认不支持直接调用。
    -- 3. 如果需要调用其它泛型函数，必须用 xlua.get_generic_method(类, "函数名") 得到泛型方法，再指定泛型类型后调用。
    -- 4. 泛型成员方法调用时，第一个参数要传对象本身；静态方法不用传对象。
    -- 5. 注意：这种方式在不同的打包方式下（Mono/IL2CPP）支持情况不同，值类型泛型参数只有 C# 侧已调用过同类型泛型参数时，Lua 才能用。
    -- 6. 建议仅了解此用法，实际项目中谨慎使用。
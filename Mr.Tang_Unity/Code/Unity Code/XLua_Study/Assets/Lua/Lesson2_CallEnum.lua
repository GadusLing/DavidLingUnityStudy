print("*********Lua调用C#枚举相关知识点***********")

-- 枚举调用
-- 调用Uinty中的枚举
-- 枚举调用规则和类调用规则类似
-- 1.通过CS.命名空间.枚举名.枚举值名    访问枚举值

PrimitiveType = CS.UnityEngine.PrimitiveType -- Unity中有个CratePrimitive函数，可以创建基本类型的物体 其中PrimitiveType就是一个枚举 包括立方体 球体等
-- 我们为PrimitiveType取一个别名 方便后续调用
GameObject = CS.UnityEngine.GameObject -- 给GameObject也取个别名

local obj = GameObject.CreatePrimitive(PrimitiveType.Cube) -- 创建了一个立方体

-- 自定义枚举 
E_MyEnum = CS.E_MyEnum -- 取个别名

local c = E_MyEnum.Idle -- 访问自定义枚举的值
print(c)

-- 枚举转换相关
-- 数值转枚举
local a = E_MyEnum.__CastFrom(1) -- 把数值1转换为枚举类型  __CastFrom是xlua帮我们自动生成的转换函数 可以把数值转换为对应的枚举类型
print(a)

--字符串转枚举
local b = E_MyEnum.__CastFrom("Attack") -- 把字符串"Move"转换为枚举类型
print(b)
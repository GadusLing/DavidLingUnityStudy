print("------------------ ToLua访问CSharp的枚举 ------------------")

-- 枚举的调用规则 和 类的调用规则是一样的
-- 命名空间.枚举名.枚举成员
-- 也支持取别名

-- 调用unity自带的枚举
-- 如果报错 需要在CustomSettings文件中添加枚举类型
PrimtiveType = UnityEngine.PrimitiveType
GameObject = UnityEngine.GameObject
local obj = GameObject.CreatePrimitive(PrimtiveType.Cube)

-- 调用自定义的枚举
local c = E_MyEnum.Idle
local d = E_MyEnum.Move
print("E_MyEnum.Idle =", c) --打印出来是个userdata类型，还记的userdata吗？lua中的一个语言类型
-- lua中的基础类型有nil, boolean, number, string, function, table, thread, userdata
-- userdata 是 Lua 里的一种类型，用来承载外部语言（C/C++、C#、Java、Python 绑定等）传入 Lua 的自定义对象或数据

if(c == d) then
    print("枚举相等")
else
    print("枚举不等")
end

print("枚举转字符串")
print("c =", tostring(c))
print("枚举转数字")
print(c:ToInt()) -- 枚举底层是整型，可以转成数字
print(d:ToInt())

print("数字转枚举")
print(tostring(E_MyEnum.IntToEnum(0))) -- 0对应Idle
print(tostring(E_MyEnum.IntToEnum(1))) -- 1对应Move

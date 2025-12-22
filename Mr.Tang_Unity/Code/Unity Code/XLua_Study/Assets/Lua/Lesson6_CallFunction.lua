print("*********Lua调用C# 函数重载 相关知识点***********")

local obj = CS.Lesson6()

-- 虽然lua不支持写函数重载
-- 但是C#支持函数重载 所以在lua中调用C#的重载函数时 根据传入参数的不同 会自动匹配对应的重载版本
print(obj:Calc())
print(obj:Calc(15, 1))

print(obj:Calc(10))
print(obj:Calc(10.2)) -- 当这里是单float参数时这里为啥会是0呢？ 因为和上面的单int参数产生了二义性 导致匹配失败 返回默认值0
-- Lua 虽然支持调用 C# 的重载函数
-- 但是因为 Lua 中的数值类型只有 Number
-- 对 C# 中多精度的重载函数支持不好，容易分不清
-- 比如上面这个例子中 在C#中有两个单参数的重载版本 一个int 一个float 
-- 模拟过程：Number 是 double 精度 此时有一个int 一个 float  
--          存在多个单参数重载，且都是数值类型(不是像int和string的对比) 仅在精度上有区别  XLua 无法唯一确定目标，判定为歧义调用
--          XLua 的歧义解决策略： 当存在 多个「单参数重载」， 且 Lua number 无法 唯一、确定地 绑定到其中一个时 直接走失败分支
--          失败分支的行为是：不抛异常 不警告 返回默认值对 Number(double) 来说，默认值就是：0 所以返回了0
--          为什么 XLua 要这么设计？Lua 是动态语言，没有“数值精度语义” XLua 不敢替你“猜”你想用哪个重载 所以它宁愿：不选 不抛异常 返回默认值

-- 那如果我们在float 后面再加个string参数呢？ 这样就改变了匹配场景
-- 尝试之后我们发现会返回了 10.199999809265 这是因为 10.2 作为 double 类型传入时 会匹配到 float 重载版本 但是 float 精度不够 导致精度损失了
-- 那如果把float改成double呢？ 结果是返回了10.2 说明 double 类型匹配了 也印证了我们上面的分析

-- 那该如何解决这个问题呢？
-- 解决重载函数参数含糊的问题
-- xLua 提供了解决方案：反射机制
-- 但是这种方法只做了解，不到万不得已尽量别用 效率低 我们自己写逻辑就尽量不要写 单参数 的多精度重载
-- Type 是反射的关键类
local m1 = typeof(CS.Lesson6):GetMethod("Calc", {typeof(CS.System.Int32)})
local m2 = typeof(CS.Lesson6):GetMethod("Calc", {typeof(CS.System.Single)})

local f1 = xlua.tofunction(m1)
local f2 = xlua.tofunction(m2)

print(f1(obj, 10))        -- 调用 int 版本
print(f2(obj, 10.2))      -- 调用 float 版本
-- 还是那句话 了解即可 能不用就不用 又麻烦 又低效 不如换个函数名的事儿



print("------------------ ToLua访问CSharp的ref 和 out ------------------")

-- tolua和Xlua中对ref 和 out函数使用基本类似
-- 通过多返回值的形式来处理的
-- 如果 是out和ref函数
-- 第一个返回值是函数的默认返回值
-- 之后的返回值是 out 和 ref 对应的结果 从左到右 一一对应
local obj = Lesson5()

-- 调用 ref 函数

print(obj:RefFunc(10, 0, 0, 1))
local a,b,c = obj:RefFunc(10, 0, 0, 1)
print("ref 结果 a:"..a.." b:"..b.." c:"..c)

obj:OutFunc(20, 0, 0, 1) -- xlua里面out占位符是可以省略不写的，但是tolua不行

print(obj:OutFunc(20, nil, nil, 30))
local d,e,f = obj:OutFunc(20, nil, nil, 30)
print("out 结果 d:"..d.." e:"..e.." f:"..f)

print(obj:RefOutFunc(20, nil, 1))
local g, h, i = obj:RefOutFunc(20, nil, 1)
print("ref out 结果 g:"..g.." h:"..h.." i:"..i)

-- 这里另外提个点 tolua的官方文档中基本都在强调out的使用，ref基本没怎么提，只是为了学习时和xlua对并，这里写上表示ref也能用
-- 实际上如果out不能省略传参，而必须要占位的话，那out和ref在使用上就基本没区别了，估计也是因为这个原因，tolua的文档中才没怎么提ref的使用
-- 所以如果要使用ref out类的参数，tolua就可以考虑都用out








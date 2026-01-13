print("------------------ ToLua访问CSharp的委托和事件------------------")
local obj = Lesson7()

-- 委托是用来存放函数指针的变量的
-- 要使用使用C#中的委托 就是用来装lua函数的
-- 我们可以把一个Lua函数赋值给CSharp的委托变量
-- 然后CSharp调用这个委托变量时 就会调用Lua函数
local fun = function()
    print("Lua中的函数fun被调用了")
end

print("------------------ 委托中加函数------------------")
-- Lua中没有复合运算符 所以不能用 += -= 只能 +
-- 如果是第一次往委托中加函数 因为是nil 所以不能直接用+ 要先用 = 赋值
obj.del = fun
obj.del = obj.del + fun -- 再次添加函数 这里就是第二次了 所以能用+
obj.del = obj.del + function()
    print("匿名函数被调用了")
end

-- xlua 中执行 是用 obj.del() 来调用委托的
-- 而toLua中 没有办法直接执行委托 所以我们在CSharp中写了一个DoDel方法来执行委托
obj:DoDel()

print("------------------ 委托中减函数------------------")
obj.del = obj.del - fun
obj.del = obj.del - fun
obj:DoDel()

print("------------------ 委托中清空函数------------------")
obj.del = nil
obj:DoDel()
obj.del = fun
obj:DoDel()


local fun2 = function()
    print("事件加的函数")
end
print("------------------ 事件中加函数------------------")
-- Xlua中 事件的添加和删除 用 对象:事件名 ("+/-", 函数)
-- tolua中事件加减函数和委托一样
-- obj.eventAction = fun2  -- 这里注意 事件不能直接= 因为遵循C#事件的语法 只能用+= -=
obj.eventAction = obj.eventAction + fun2
obj.eventAction = obj.eventAction + function()
    print("事件匿名函数被调用了")
end

obj:DoEvent()
-- 这里顺便复习一下C#中事件和委托的区别
-- 委托是一个类型，可以像普通变量一样直接赋值（=）、加函数（+）、减函数（-）、清空（= nil），外部代码有完全的读写权限。
-- 事件（event）是基于委托的语法糖，加了 event 修饰后，只允许外部用 +=、-= 订阅/取消，不能直接赋值或清空，防止外部误操作，保证封装和安全。

print("------------------ 事件中减函数------------------")
obj.eventAction = obj.eventAction - fun2
obj:DoEvent()

print("------------------ 事件中清空函数------------------")
obj:ClearEvent() -- 上面说了，事件不能直接赋值=，所以不能用 = nil， 要能通过C#中写一个方法清空
obj:DoEvent()




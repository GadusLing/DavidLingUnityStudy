print("**********泛型类替换**********")

-- 泛型类T是可变的，那lua中该如何替换呢？
-- 要一个一个类型去替换！
xlua.hotfix(CS.HotfixTest2(CS.System.String), {
    Test = function(self, str)
        print("这是被HotFix替换后的泛型类Test函数，类型是string，参数是：" .. str)
    end
})

xlua.hotfix(CS.HotfixTest2(CS.System.Int32), {
    Test = function(self, num)
        print("这是被HotFix替换后的泛型类Test函数，类型是int，参数是：" .. num)
    end
})
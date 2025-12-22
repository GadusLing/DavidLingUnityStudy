print("**********第一个热补丁**********")

-- lua中 热补丁代码固定写法
-- xlua.hotfix(类, "函数名", lua函数)

xlua.hotfix(CS.HotFixMain, "add", function(self, a, b) -- 成员方法第一个参数传self
    return a + b
end)

xlua.hotfix(CS.HotFixMain, "Speak", function(str) -- 静态方法不用传self
    print("这是Lua热补丁后的Speak方法" .. str)
end)

-- 到这了热补丁的基本代码就结束了，但测试还是报错 要进行四步操作
-- 1. 加特性 在C#代码中 添加 [Hotfix] 特性 标记需要热补丁的类
-- 2. 加宏 在File → Build Setting → Other Setting → Scripting Define Symbols 添加 宏定义 HOTFIX_ENABLE 标记开启热补丁功能 不用热补丁就不要开 不然每次改代码都要注入
-- 3. 生成代码 在Unity菜单栏 XLua -> Generate Code 生成热补丁相关代码
-- 4. hotfix注入 在C#工具栏 XLua -> Hotfix Injection in Editor 注入热补丁代码 -- 这一步要把Tool工具文件夹拷贝到Assets的同级目录下才能成功 不然报错

-- Tips：热补丁有个缺点 一旦修改了C#代码 就需要重写执行上面的第四步 注入热补丁代码

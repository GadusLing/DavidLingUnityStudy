// See https://aka.ms/new-console-template for more information
//Console.WriteLine("随机数");
//#region 知识点一 产生随机数对象
////固定写法
//// Random 随机数变量名 = new Random();
//Random r = new Random();
//#endregion

//#region 知识点二 生成随机数
//int i = r.Next(); //生成一个非负数的随机数
//Console.WriteLine(i);

//i = r.Next(100); // 生成一个 0~99的随机数 左边始终是0 左包含  右边是100 右不包含
//Console.WriteLine(i);

//i = r.Next(5, 100); // 生成一个 5到99的随机数 左包含 右不包含
//Console.WriteLine(i);
//#endregion


Console.WriteLine("随机数练习题");
//打小怪兽
//攻击力为8~12之间的一个值
//小怪兽防御为10，血量为20
//在控制台中通过打印信息表现唐老狮打小怪兽的过程
//描述小怪兽的掉血情况
//伤害计算公式：攻击力小于防御力时，减血为0，否则减血攻击力和防御力的差值

//知识点：循环、随机数等等

int monsterDef = 10;
int monsterHp = 20;
int LDWAtk = 0;
Random r = new Random();

while (true)
{
    LDWAtk = r.Next(8, 20);
    if (LDWAtk > monsterDef)
    {
        //减血攻击力和防御力的差值
        //monsterHp = monsterHp - (LDWAtk - monsterDef);
        monsterHp -= LDWAtk - monsterDef;
        Console.WriteLine("凌大伟本次攻击力为{0},造成{1}伤害,怪物剩{2}血",
            LDWAtk, LDWAtk - monsterDef, monsterHp);
        if (monsterHp <= 0)
        {
            break;
        }
    }
    else
    {
        Console.WriteLine("凌大伟本次攻击力为{0},不足以造成伤害", LDWAtk);
    }

    Console.WriteLine("请按任意键继续攻击");
    Console.ReadKey(true);
    Console.Clear();
}

Console.WriteLine("怪物已死亡");

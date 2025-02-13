// See https://aka.ms/new-console-template for more information
using System;
using System.Security.Principal;

/*namespace Lesson1_枚举
{

    #region 知识点一 基本概念

    #region 1.枚举是什么 
    //枚举是一个比较特别的存在
    //它是一个被命名的整形常量的集合
    //一般用它来表示 状态 类型 等等
    #endregion

    #region 2.申明枚举 和 申明枚举变量
    //注意：申明枚举 和 申明枚举变量 是两个概念
    //申明枚举：     相当于是 创建一个自定义的枚举类型
    //申明枚举变量： 使用申明的自定义枚举类型 创建一个枚举变量
    #endregion

    #region 3.申明枚举语法
    // 枚举名 以E或者E_开头 作为我们的命名规范
    //enum E_自定义枚举名
    //{
    //    自定义枚举项名字, //枚举中包裹的 整形常量  第一个默认值是0 下面会依次累加
    //    自定义枚举项名字1,//1
    //    自定义枚举项名字2,//2
    //}

    //enum E_自定义枚举名
    //{
    //    自定义枚举项名字 = 5, //第一个枚举项的默认值 变成5了 
    //    自定义枚举项名字1,// 6
    //    自定义枚举项名字2 = 100,
    //    自定义枚举项名字3,//101
    //    自定义枚举项名字4,//102
    //}

    #endregion

    #endregion

    #region 知识点二 在哪里申明枚举
    //1.namespace语句块中（常用）
    //2.class语句块中 struct语句块中
    //注意：枚举不能在函数语句块中申明！！！

    enum E_MonsterType
    {
        Normal,//0


        Boss,//1
    }

    enum E_PlayerType
    {
        Main,
        Other,
    }


    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("枚举");

            #region 知识点三 枚举的使用
            //申明枚举变量
            //自定义的枚举类型  变量名 = 默认值;(自定义的枚举类型.枚举项)
            E_PlayerType playerType = E_PlayerType.Other;

            if (playerType == E_PlayerType.Main)
            {
                Console.WriteLine("主玩家逻辑");
            }
            else if (playerType == E_PlayerType.Other)
            {
                Console.WriteLine("其它玩家逻辑");
            }

            //枚举和switch是天生一对
            E_MonsterType monsterType = E_MonsterType.Boss;
            switch (monsterType)
            {
                case E_MonsterType.Normal:
                //Console.WriteLine("普通怪物逻辑");
                //break;
                case E_MonsterType.Boss:
                    Console.WriteLine("Boss逻辑");
                    break;
                default:
                    break;
            }

            #endregion


            #region 知识点四 枚举的类型转换
            // 1.枚举和int互转
            int i = (int)playerType;
            Console.WriteLine(i);
            //int 转枚举
            playerType = 0;

            // 2.枚举和string相互转换
            string str = playerType.ToString();
            Console.WriteLine(str);

            //把string转成枚举呢
            //Parse后 第一个参数 ：你要转为的是哪个 枚举类型 第二个参数：用于转换的对应枚举项的字符串
            //转换完毕后 是一个通用的类型 我们需要用括号强转成我们想要的目标枚举类型
            playerType = (E_PlayerType)Enum.Parse(typeof(E_PlayerType), "Other");
            Console.WriteLine(playerType);

            #endregion

            #region 知识点五 枚举的作用
            //在游戏开发中，对象很多时候 会有许多的状态
            //比如玩家 有一个动作状态 我们需要用一个变量或者标识 来表示当前玩家处于的是哪种状态
            //综合考虑 可能会使用 int 来表示他的状态
            // 1 行走 2 待机 3 跑步 4 跳跃。。。。。。。等等

            //枚举可以帮助我们 清晰的分清楚状态的含义
            #endregion
        }
    }
}*/

// 作业

/// <summary>
/// QQ状态枚举
/// </summary>
enum E_QQstate
{   /// <summary>
    /// 在线
    /// </summary>
    Online,
    Leave,
    Busy,
    Invisible,
}

/// <summary>
/// 咖啡杯大小
/// </summary>
enum E_coffee
{
    Medium, 
    large, 
    superLarge,
}

enum E_sex
{
    man,
    women,
}

enum E_occupation
{
    /// <summary>
    /// 战士
    /// </summary>
    warrior,
    /// <summary>
    /// 猎人
    /// </summary>
    hunter,
    /// <summary>
    /// 法师
    /// </summary>
    master,
}


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("枚举练习题");
        #region 练习题一
        ////定义QQ状态的枚举，并提示用户选择一个在线状态，我们接受输入的数字，并将其转换成枚举类型
        //try
        //{
        //    Console.WriteLine("请输入在线状态{0在线, 1离开, 2忙碌, 3隐身}");
        //    int state = int.Parse(Console.ReadLine());
        //    E_QQstate nowState = (E_QQstate)state;
        //    Console.WriteLine(nowState);
        //}
        //catch
        //{
        //    Console.WriteLine("请输入数字");
        //}
        ////try catch写法,可以捕获非数字输入的异常,但是比如99这种输入就会麻爪,可以通过增加state is >= 0 and <= 3条件来解决

        //// 不如这个老手级写法,只读一次按键输出,三目判断是否合法区间
        //Console.WriteLine("输入状态(0:在线,1:离开,2:忙碌,3:隐身):");
        //int state = Console.ReadKey(true).KeyChar - '0';
        //Console.WriteLine(state is >= 0 and <= 3 ? (E_QQstate)state : "请输入 0-3 之间的数字");
        #endregion

        #region 练习题二
        //用户去星巴克买咖啡，分为中杯（35元），大杯（40元），超大杯（43元），
        //请用户选择要购买的类型，用户选择后，打印：您购买了xxx咖啡，花费了xx元
        //例如：你购买了中杯咖啡，花费了35元
        //Console.WriteLine("请选择要购买的容量: 0-中杯 (35元), 1-大杯 (40元), 2-超大杯 (43元)");
        //int nowCoffee = Console.ReadKey(true).KeyChar - '0';  // 读取并转换为整数
        //switch (nowCoffee)
        //{
        //    case 0:
        //        Console.WriteLine("您购买了中杯咖啡，花费了35元");
        //        break;
        //    case 1:
        //        Console.WriteLine("您购买了大杯咖啡，花费了40元");
        //        break;
        //    case 2:
        //        Console.WriteLine("您购买了超大杯咖啡，花费了43元");
        //        break;
        //    default:
        //        Console.WriteLine("输入错误，请输入 0、1 或 2 选择咖啡大小！");
        //        break;
        //}
        #endregion

        #region 练习题三
        //请用户选择英雄性别与英雄职业，最后打印英雄的基本属性（攻击力，防御力，技能）
        //性别：
        //男（攻击力 + 50，防御力 + 100）
        //女（攻击力 + 150，防御力 + 20）
        //职业：
        //战士（攻击力 + 20，防御力 + 100，技能：冲锋）
        //猎人（攻击力 + 120，防御力 + 30，技能：假死）
        //法师（攻击力 + 200，防御力 + 10，技能：奥术冲击）

        //举例打印：你选择了“女性法师”，攻击力：350，防御力：30，职业技能：奥术冲击

        int ATK = 0, DEF = 0;
        string playerSex;
        Console.WriteLine("输入数字,选择你的性别: 0-男, 1-女");
        int nowSex = Console.ReadKey(true).KeyChar - '0';  // 获取性别输入
        switch (nowSex)
        {
            case 0:
                ATK += 50;
                DEF += 100;
                playerSex = "你选择了\"男性";
                break;
            case 1:
                ATK += 150;
                DEF += 20;
                playerSex = "你选择了\"女性";
                break;
            default:
                Console.WriteLine("请输入正确的数字,0 or 1");
                return;  // 输入错误直接返回，不继续执行
        }

        Console.WriteLine("输入数字,选择你的职业: 0-战士, 1-猎人, 2-法师 ");
        int nowOccupation = Console.ReadKey(true).KeyChar - '0';  // 获取职业输入
        switch (nowOccupation)
        {
            case 0://其实这里case里面应该用case E_Occupation.Warrior:这样的形式来做,才能更好的发挥枚举的作用,提高可读性,这里知道就行了,偷个懒
                ATK += 20;
                DEF += 100;
                Console.WriteLine(playerSex + "战士\"，攻击力：" + ATK + "，防御力：" + DEF + "，职业技能：力量冲击");
                break;
            case 1:
                ATK += 120;
                DEF += 30;
                Console.WriteLine(playerSex + "猎人\"，攻击力：" + ATK + "，防御力：" + DEF + "，职业技能：精准射击");
                break;
            case 2:
                ATK += 200;
                DEF += 10;
                Console.WriteLine(playerSex + "法师\"，攻击力：" + ATK + "，防御力：" + DEF + "，职业技能：奥术冲击");
                break;
            default:
                Console.WriteLine("请输入正确的数字,0 or 1 or 2");
                break;
        }
        #endregion
    }
}







// 题目1: 将下面的食物进行分类，可重复
// 分类: 动物, 人, 交通工具, 植物, 天体
var animals = new List<string> { "机器人", "机器", "人", "猫" };
var people = new List<string> { "张阿姨", "隔壁老王" };
var vehicles = new List<string> { "汽车", "飞机" };
var plants = new List<string> { "向日葵", "菊花", "荷花" };
var celestialBodies = new List<string> { "太阳", "星星" };

/*// 题目2: A目前等于多少?
GameObject A = new GameObject();
GameObject B = A;
B = null;
// A 仍然等于一个新的 GameObjectO 实例*/

// 题目3:A和B有什么关系?
GameObject A = new GameObject();
GameObject B = A;
B = new GameObject();
// A 和 B 都是 GameObject 的引用类型，B 被重新赋值为一个新的 GameObject 实例后，A 仍然指向原来的实例。

namespace 静态成员作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: 1. 请说出const和static的区别
            // - const关键字的特点：在编译时就确定了值，不能被修改
            // - static关键字的特点：属于类而不是实例，可以通过类名直接访问
            // - 两者的主要区别：
            // const必须初始化且值不可变，而static可以在运行时被修改
            // const只能修饰变量，而static可以修饰字段、方法、属性等
            // 访问修饰符要写在const之前，而static则无所谓


            // TODO: 2. 请用静态成员相关知识实现
            // 要求：一个类对象，在整个应用程序的生命周期中，有且仅会有一个该对象的存在
            // 不能在外部实例化，直接通过类类名就能够获得到唯一一个对象

            // 提示：这是单例模式的实现
            // - 构造函数应该是私有的
            // - 需要一个静态字段存储唯一实例
            // - 需要一个静态方法获取实例

            Console.WriteLine(SingletonClass.T.Value);
        }
    }

    //TODO: 在这里实现单例类
    class SingletonClass
    {
        private SingletonClass() { }

        public static SingletonClass T
        {
            get
            {
                return _instance;
            }
        }

        private static SingletonClass _instance = new SingletonClass();
        public int Value = 10;
    }
}

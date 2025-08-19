namespace 俄罗斯方块.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建游戏实例
            Game game = new Game();

            // 启动游戏主循环,Start()里包含了初始化
            game.Start();
        }
    }
}

// See https://aka.ms/new-console-template for more information


Console.WriteLine("Hello, World!");


Console.WriteLine("你的名字");

Console.ReadLine();

Console.WriteLine("你的年龄");

Console.ReadLine();

Console.WriteLine("你的班级");

Console.ReadLine();

Console.WriteLine("你最喜欢的运动");

Console.ReadLine();

Console.WriteLine("哈哈，好巧我也喜欢这个运动");




int size = 10;

for (int i = 0; i < size; i++)  // 遍历每一行
{
    for (int j = 0; j < size; j++)  // 遍历每一列
    {
        // 判断边界：第一行、最后一行、第一列、最后一列
        if (i == 0 || i == size - 1 || j == 0 || j == size - 1)
        {
            Console.Write("*");  // 边界位置输出星号
        }
        else
        {
            Console.Write(" ");  // 内部位置输出空格
        }
    }
    Console.WriteLine();  // 输出完一行后换行
}
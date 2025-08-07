namespace 静态类和静态构造函数作业
{
    /// <summary>
    /// 数学计算工具类 - 提供各种几何图形的计算方法
    /// </summary>
    static class MathCalculator
    {
        // TODO: 静态构造函数
        // 用于初始化静态成员变量（如常量、配置等）
        static MathCalculator()
        {}

        // TODO: 圆形相关计算
        // - 计算圆面积的方法  Πr^2
        public static double CalculateCircleArea(double radius)
        {
            return _Pi * radius * radius;
        }
        // - 计算圆周长的方法 2Πr
        public static double CalculateCircleCircumference(double radius)
        {
            return 2 * _Pi * radius;
        }

        // TODO: 矩形相关计算
        // - 计算矩形面积的方法  宽 * 高
        public static double CalculateRectangleArea(double width, double height)
        {
            return width * height;
        }
        // - 计算矩形周长的方法  2 * (宽 + 高)
        public static double CalculateRectanglePerimeter(double width, double height)
        {
            return 2 * (width + height);
        }

        // TODO: 数值处理方法
        // - 取绝对值的方法
        public static double GetAbsoluteValue(double number)
        {
            return number < 0 ? -number : number;
        }

        static double _Pi = 3.1415926;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            // TODO: 测试静态类的各种计算方法
            // - 测试圆形计算
            // - 测试矩形计算  
            // - 测试数值处理
            // - 输出计算结果
            double circleRadius = 5.0;
            double circleArea = MathCalculator.CalculateCircleArea(circleRadius);
            double circleCircumference = MathCalculator.CalculateCircleCircumference(circleRadius);
            Console.WriteLine($"Circle Area: {circleArea}");
            Console.WriteLine($"Circle Circumference: {circleCircumference}");
            double rectangleWidth = 4.0;
            double rectangleHeight = 6.0;
            double rectangleArea = MathCalculator.CalculateRectangleArea(rectangleWidth, rectangleHeight);
            double rectanglePerimeter = MathCalculator.CalculateRectanglePerimeter(rectangleWidth, rectangleHeight);
            Console.WriteLine($"Rectangle Area: {rectangleArea}");
            Console.WriteLine($"Rectangle Perimeter: {rectanglePerimeter}");
            double number = -10.5;
            double absoluteValue = MathCalculator.GetAbsoluteValue(number);
            Console.WriteLine($"Absolute Value of {number}: {absoluteValue}");

        }
    }
}

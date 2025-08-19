namespace Dictionary作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目1：壹贰叁肆伍陆柒捌玖拾
            // 使用字典存储0~9的数字对应的大写文字
            // 提示用户输入一个不超过三位的数，提供一个方法，返回数的大写
            // 例如：306，返回叁零陆

            try
            {
                Console.WriteLine("请输入一个不超过三位的数字：");
                Console.WriteLine(GetInfo(int.Parse(Console.ReadLine())));
            }
            catch
            {
                Console.WriteLine("输入无效，请输入一个不超过三位的数字。");
            }


            static string GetInfo(int num)
            {
                // 添加位数验证
                if (num < 0 || num > 999)
                {
                    throw new ArgumentException("数字必须在0到999之间");
                }
                Dictionary<int, string> numberWords = new Dictionary<int, string>
                {
                    { 0, "零" },
                    { 1, "壹" },
                    { 2, "贰" },
                    { 3, "叁" },
                    { 4, "肆" },
                    { 5, "伍" },
                    { 6, "陆" },
                    { 7, "柒" },
                    { 8, "捌" },
                    { 9, "玖" }
                };
                string result = "";
                if (num == 0)
                {
                    return numberWords[0];
                }
                while (num > 0)
                {
                    int digit = num % 10; // 获取最后一位数字
                    result = numberWords[digit] + result; // 拼接结果
                    num /= 10; // 去掉最后一位数字
                }
                return result;
            }

            // 题目2：计算每个字母出现的次数
            // 计算每个字母出现的次数 "Welcome to Unity World！"，使用字典
            // 存储，最后遍历整个字典，不区分大小写
            Dictionary<char, int> NOOL = new Dictionary<char, int>();
            string input = "Welcome to Unity World！";
            foreach (char c in input.ToLower())
            {
                if (NOOL.ContainsKey(c))
                {
                    NOOL[c]++;
                }
                else
                {
                    NOOL[c] = 1;
                }
            }
            foreach (var item in NOOL)
            {
                Console.WriteLine($"字母 '{item.Key}' 出现了 {item.Value} 次");
            }
        }
    }
}

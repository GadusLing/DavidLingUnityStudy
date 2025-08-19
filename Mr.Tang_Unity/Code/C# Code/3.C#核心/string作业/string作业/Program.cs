namespace string作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目1：请写出string中提供的截取和替换对应的函数名
            // substring() 和 Replace()
            string str = "Hello, World!";
            Console.WriteLine(str.Replace(str.Substring(0, 5), "Hi"));

            // 题目2：请将字符串1|2|3|4|5|6|7
            //       变为      2|3|4|5|6|7|8
            //       并输出
            //       (使用字符串切割的方法)
            string input = "1|2|3|4|5|6|7";
            string[] parts = input.Split('|');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = (int.Parse(parts[i]) + 1).ToString();
            }
            Console.WriteLine(string.Join("|", parts));

            // 题目3：String和string、Int32和int、Int16和short、Int64和long它们的区别是什么？
            // String和string是同一个类型，string是C#的别名，Int32和int、Int16和short、Int64和long也是同理。


            // 题目4：string str = null;
            //       str = "123";
            //       string str2 = str;
            //       str2 = "321";
            //       str2 += "123";
            //       请问，上面这段代码，分配了多少个新的堆空间
            //       3个
            //       解释：str = "123" 分配了一个新的堆空间，str2 = str 只是引用了这个空间，str2 = "321" 又分配了一个新的堆空间，
            //       str2 += "123" 又分配了一个新的堆空间，所以总共分配了3个新的堆空间。

            // 题目5：编写一个函数，将输入的字符串反转，不要使用中间商，你必须原地修改输入数组。交换过程中不使用额外空间
            //       比如：输入{ 'h', 'e', 'l', 'l', 'o' }  输出
            //            { 'o', 'l', 'l', 'e', 'h' }
            Console.WriteLine("请输入要反转的字符串（以回车结束）：");
            string inputArray = Console.ReadLine();
            Console.WriteLine(inputArray.Reverse().ToArray());

            // 不用用额外空间的反转方法
            char[] charArray = inputArray.ToCharArray();
            int left = 0;
            int right = charArray.Length - 1;
            while (left < right)
            {
                // 交换字符
                charArray[left] = (char)(charArray[left] + charArray[right]);
                charArray[right] = (char)(charArray[left] - charArray[right]);
                charArray[left] = (char)(charArray[left] - charArray[right]);
                left++;
                right--;
            }
            Console.WriteLine(new string(charArray));
        }
    }
}

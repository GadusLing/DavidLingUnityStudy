using System;
using System.Text;

namespace StringBuilder作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //请描述string和stringbuilder的区别
            //string是不可变的，每次修改都会创建一个新的字符串对象，消耗更多内存和性能。
            //stringbuilder是可变的，允许在原有对象上进行修改，减少了内存分配和复制的开销。
            //1.string相对stringbuilder更容易产生垃圾每次修改拼接都会产生垃圾
            //2.string相对stringbuilder
            //更加灵活因为它提供了更多的方法供使用
            //如何选择他们两
            //需要频繁修改拼接的字符串可以使用stringbuilder
            //需要使用string独特的一些方法来处理一些特殊逻辑时可以使用string

            //如何优化内存
            //内存优化 从两个方面去解答
            //1.如何节约内存
            //2.如何尽量少的GC（垃圾回收）
            //少new对象少产生垃圾
            //合理使用static
            //合理使用string和stringbuilder
            //使用StringBuilder来构建字符串，避免频繁的字符串拼接操作。
            //使用StringBuilder的Append方法来添加字符串，避免了创建多个中间字符串对象。
            //使用StringBuilder的Clear方法来清空内容，而不是创建新的StringBuilder对象。

        }
    }
}

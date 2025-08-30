using UnityEngine;

// 拓展方法工具类，专门为 Transform 类型添加新功能
public static class TransformExtensions
{
    /*
     * 拓展方法 SortChildrenByName
     * 功能：将指定 Transform 的所有子物体按名字长度排序，并重新排列层级顺序
     * 参数说明：
     *   this Transform transform —— 要排序的父物体（拓展方法的目标对象）
     *   bool ascending —— 是否升序排列（true：名字短的在前，false：名字长的在前），默认值为 true
     * 用法示例：
     *   transform.SortChildrenByName(); // 升序排列
     *   transform.SortChildrenByName(false); // 降序排列
     */
    // 拓展方法必须：
    // 1. 在静态类中定义
    // 2. 是静态方法
    // 3. 第一个参数使用 this 关键字，表示要拓展的类型
    public static void SortChildrenByName(this Transform transform, bool ascending = true)
    {
        // 1. 创建一个 Transform 数组，用于存放所有子物体
        // transform.childCount 表示子物体的数量
        var children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            // transform.GetChild(i) 获取第 i 个子物体
            children[i] = transform.GetChild(i);
        }

        /*
         * 2. 对 children 数组进行排序
         * System.Array.Sort 有多个重载，常用的有：
         *   - Array.Sort(T[] array)：对数组整体排序，要求元素实现 IComparable 接口
         *   - Array.Sort(T[] array, Comparison<T> comparison)：用自定义比较器排序
         * 这里用的是第二种重载，传入一个 lambda 表达式作为比较器
         *
         * lambda 表达式写法：(a, b) => {...}
         *   - a, b：每次比较的两个 Transform 对象
         *   - =>：表示“返回”
         *   - a.name.Length：a 的名字长度
         *   - b.name.Length：b 的名字长度
         *   - CompareTo 方法：比较两个整数（名字长度），返回负数/零/正数
         *      如果当前对象 小于 参数对象，返回 负数（通常是 -1）
         •	    如果当前对象 等于 参数对象，返回 0
         •	    如果当前对象 大于 参数对象，返回 正数（通常是 1）
         *     - 负数：a 在 b 前面
         *     - 零：顺序不变
         *     - 正数：a 在 b 后面
         *   - ascending：是否升序
         *     - true：短名字在前
         *     - false：长名字在前
         */
        System.Array.Sort(children, (a, b) =>
        {
            // 如果 ascending 为 true，按名字长度升序排列
            // 如果 ascending 为 false，按名字长度降序排列
            return ascending ?
                a.name.Length.CompareTo(b.name.Length) : // 升序：短的在前
                b.name.Length.CompareTo(a.name.Length);   // 降序：长的在前
        });

        /*
         * 3. 重新设置子物体的层级顺序
         * SetSiblingIndex(i)：把当前子物体放到第 i 个位置
         * 这样 Unity 层级视图中的顺序也会发生变化
         */
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    /// <summary>
    /// 拓展方法：递归查找指定名字的子物体（包括所有层级）
    /// </summary>
    /// <param name="father">要查找的父物体</param>
    /// <param name="childrenName">目标子物体的名字</param>
    /// <returns>找到的 Transform 对象，如果没找到返回 null</returns>
    public static Transform DepthFindChildren(this Transform father, string childrenName)
    {
        // 1. 先在当前层级查找（只查找直接子物体）
        Transform foundChild = father.Find(childrenName);
        if (foundChild != null)
        {
            // 找到直接子物体，返回
            return foundChild;
        }

        // 2. 没找到则递归查找所有子物体
        for (int i = 0; i < father.childCount; i++)
        {
            Transform child = father.GetChild(i);
            // 递归查找子物体的子物体
            Transform result = child.DepthFindChildren(childrenName);
            if (result != null)
            {
                // 找到就返回，不再继续查找
                return result;
            }
        }

        // 3. 全部查找完没找到，返回 null
        return null;
    }
}
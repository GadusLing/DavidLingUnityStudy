using System;

namespace 顺序储存和链式储存作业
{
    /*
     * 作业题目：
     * 1. 请说出常用的数据结构有哪些
     * 数组 链表 堆 栈 队列 二叉树 哈希 图  
     * 2. 请描述顺序存储和链式存储的区别
     *   * 顺序存储：数据连续存储，内存分配连续，访问速度快，但插入删除效率低。
     *   链式存储：数据分散存储，内存分配不连续，访问速度慢，但插入删除效率高。
     * 3. 请尝试自己实现一个双向链表，并提供以下方法和属性：
     *    - 数据的个数、头节点、尾节点
     *    - 增加数据到链表最后
     *    - 删除指定位置节点
     */

    /*
     * C++ 实现版本（已注释）：
     * 
     * #include <iostream>
     * using namespace std;
     * 
     * // Node 结构体（模板）
     * // 说明：
     * // - 使用模板 template<typename T>，使链表可以存放任意类型的元素（int、string、自定义类等）。
     * // - data 存放节点的值；prev 指向前一个节点；next 指向下一个节点。
     * // - 构造函数 Node(T val = T())：
     * //     - 当你创建节点时可以传入初始值 val。
     * //     - 默认参数 T() 表示如果没传值就用 T 的默认构造（例如 int -> 0，类类型 -> 调用默认构造）。
     * template<typename T>
     * struct Node {
     *     T data;         // 节点保存的数据（泛型）
     *     Node* prev;     // 指向前驱节点的裸指针（可以为 nullptr 或者指向其它节点）
     *     Node* next;     // 指向后继节点的裸指针
     *     Node(T val = T()) : data(val), prev(nullptr), next(nullptr) {}
     * };
     * 
     * // DoublyLinkedList 双向循环链表类（带头结点 / 哨兵）
     * // 设计说明（高层）：
     * // - 使用一个 head 指针指向头结点（哨兵节点）。头结点不存放有效数据，仅用于简化边界处理。
     * // - 采用循环结构：head->next 指向第一个元素，head->prev 指向最后一个元素；空表时 head->next == head。
     * // - 优点：插入/删除时不需要大量判断 nullptr，边界处理统一。
     * // - 注意：C++ 中需要手动管理内存（new/delete）。
     * template<typename T>
     * class DoublyLinkedList {
     * private:
     *     Node<T>* head; // 指向哨兵节点（该节点不存实际数据，仅作链表统一入口）
     * 
     * public:
     *     // 构造函数
     *     // - 创建一个哨兵节点 head，并让 head 的 next/prev 都指向自己（表示空的循环链表）。
     *     // - 为什么要指向自己？
     *     //   - 这样可以统一处理插入/删除：无须在每次操作时检查 head 是否为 nullptr 或者检查尾节点是否存在。
     *     //   - 遍历时只要遇到 head 就能停止，避免空指针判断。
     *     DoublyLinkedList() {
     *         head = new Node<T>(); // 分配哨兵节点
     *         head->next = head;    // 空表时 self-loop
     *         head->prev = head;
     *     }
     * 
     *     // 析构函数（负责释放所有节点，避免内存泄露）
     *     // - 调用 clear() 删除除 head 之外的所有节点
     *     // - 删除完毕后再 delete head
     *     // - 为什么需要显式析构？
     *     //   - C++ 没有 GC，new 出来的内存必须对应 delete，否则会内存泄漏。
     *     ~DoublyLinkedList() {
     *         clear();      // 释放链表中所有动态分配的节点（除 head）
     *         delete head;  // 释放哨兵节点本身
     *     }
     * 
     *     // push_back
     *     // 功能：在链表尾部追加一个新节点（等价于尾插法）
     *     // 参数：T val — 要插入的数据（按值传递）
     *     // 说明与注意：
     *     // - 这里使用传值（T val）是示例；如果 T 是大对象，实际项目中建议使用 const T& val（避免复制）。
     *     // - 时间复杂度：O(1)（因为我们能直接通过 head->prev 访问尾节点）
     *     void push_back(T val) {
     *         Node<T>* node = new Node<T>(val); // 新建节点并赋值
     *         Node<T>* tail = head->prev;       // 取当前尾节点（若为空表，tail == head）
     * 
     *         // 更新指针顺序非常关键：必须保证在任何时刻链表的连接性不要被破坏太久
     *         // 以下四行完成： tail <-> node <-> head
     *         tail->next = node;    // 让旧的尾节点指向新节点
     *         node->prev = tail;    // 新节点的前驱指向旧尾
     *         node->next = head;    // 新节点的后继指向 head（循环链）
     *         head->prev = node;    // head 的 prev 指向新的尾节点
     *     }
     * 
     *     // display
     *     // 功能：从头到尾打印链表中的所有元素（跳过头结点）
     *     // 说明：
     *     // - 通过 cur != head 做循环条件，因为 head 是循环链的终止标记。
     *     // - 如果链表为空（只有哨兵），cur 初始为 head->next（等于 head），循环体一次也不会执行。
     *     // - 时间复杂度：O(n)
     *     void display() {
     *         Node<T>* cur = head->next; // 从第一个元素开始（可能就是 head 本身）
     *         while (cur != head) {      // 遍历直到回到哨兵节点
     *             cout << cur->data << " ";
     *             cur = cur->next;
     *         }
     *         cout << endl;
     *     }
     * 
     *     // clear
     *     // 功能：删除链表中所有实际节点（不删除哨兵 head）
     *     // 说明：
     *     // - 遍历每个节点并 delete，防止内存泄露。
     *     // - 删除后，把 head 恢复为 self-loop，表示空表。
     *     // - 时间复杂度：O(n)
     *     void clear() {
     *         Node<T>* cur = head->next; // 从第一个元素开始
     *         while (cur != head) {      // 遍历直到回到哨兵
     *             Node<T>* tmp = cur;    // 保存当前要删除的节点
     *             cur = cur->next;      // 先移动 cur（确保不会丢失下一个节点）
     *             delete tmp;           // 释放当前节点（因为是 new 出来的）
     *         }
     *         // 恢复 head 的空表状态
     *         head->next = head;
     *         head->prev = head;
     *     }
     * };
     * 
     * // main 用法示例
     * // - 创建一个 DoublyLinkedList<int>，插入三个元素然后打印
     * // - 这是运行示例，方便验证链表行为是否正确。
     * int main() {
     *     DoublyLinkedList<int> list;
     *     list.push_back(10); // 插入 10（尾部）
     *     list.push_back(20);
     *     list.push_back(30);
     *     list.display();     // 期望输出: 10 20 30
     *     return 0;
     * }
     */

    /*
     * Node<T> 类（泛型节点）
     * 说明：
     * - T 是泛型类型参数，可以是值类型（int, struct）或引用类型（class）。
     * - Data 存放节点的值；Prev/Next 分别指向前驱和后继节点（引用类型）。
     * - 构造函数 Node(T data = default(T))：
     *     - default(T) 表示泛型类型的默认值（例如 int -> 0，引用类型 -> null）。
     * - 与 C++ 不同：C# 使用托管堆和 GC（垃圾回收），我们不需要手动 delete。
     */
    public class Node<T>
    {
        public T Data;         // 节点数据
        public Node<T> Prev;   // 指向前驱节点的引用
        public Node<T> Next;   // 指向后继节点的引用

        public Node(T data = default(T))
        {
            Data = data;
            Prev = null;
            Next = null;
        }
    }

    /*
     * DoublyLinkedList<T> 双向循环链表（带头结点）
     * 设计说明（高层）：
     * - 使用 head 作为哨兵节点（不存有效数据），head.Next 指向第一个元素，head.Prev 指向最后一个。
     * - 空表时，head.Next == head，head.Prev == head（self-loop）。
     * - C# 的 GC 会回收不再被引用的对象，但显式断开引用可以更快让对象成为回收候选。
     */
    public class DoublyLinkedList<T>
    {
        private Node<T> head; // 哨兵节点
        private int count;    // 数据的个数

        /*
         * 构造函数
         * - 创建哨兵节点并把 Next/Prev 都指向自己，形成循环。
         */
        public DoublyLinkedList()
        {
            head = new Node<T>();
            head.Next = head;
            head.Prev = head;
            count = 0;
        }

        /*
         * 属性：数据的个数
         */
        public int Count => count;

        /*
         * 属性：头节点（第一个有效节点）
         */
        public Node<T> FirstNode => head.Next == head ? null : head.Next;

        /*
         * 属性：尾节点（最后一个有效节点）
         */
        public Node<T> LastNode => head.Prev == head ? null : head.Prev;

        /*
         * PushBack
         * 功能：在链表尾部添加一个新节点
         * 参数：T val — 要插入的值（按值传递，但引用类型只是传引用副本）
         *
         * 说明：
         * - C# 的参数传递是值传递（对于引用类型，传的是引用的副本），
         *   所以如果 T 是引用类型，PushBack 接受的是引用的副本（并不复制对象本身）。
         * - 时间复杂度：O(1)
         */
        public void PushBack(T val)
        {
            var node = new Node<T>(val); // 新建节点
            var tail = head.Prev;        // 取当前尾节点（若空表，tail == head）

            // 更新指针，顺序同 C++：tail -> node -> head
            tail.Next = node;
            node.Prev = tail;
            node.Next = head;
            head.Prev = node;
            count++;
        }

        /*
         * RemoveAt
         * 功能：删除指定位置的节点
         * 参数：int index — 要删除的节点索引（从0开始）
         * 返回：bool — 删除成功返回true，失败返回false
         */
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                return false;

            var cur = head.Next;
            for (int i = 0; i < index; i++)
            {
                cur = cur.Next;
            }

            // 断开当前节点的连接
            cur.Prev.Next = cur.Next;
            cur.Next.Prev = cur.Prev;
            
            // 断开节点的引用（帮助GC）
            cur.Prev = null;
            cur.Next = null;
            
            count--;
            return true;
        }

        /*
         * Display
         * 功能：从头到尾打印链表中的元素（跳过哨兵）
         * 说明：
         * - 使用 while(cur != head) 来控制循环停止。
         * - 如果链表为空，cur 初始就是 head，循环不会执行。
         */
        public void Display()
        {
            var cur = head.Next;
            while (cur != head)
            {
                Console.Write(cur.Data + " ");
                cur = cur.Next;
            }
            Console.WriteLine();
        }

        /*
         * Clear
         * 功能：清空链表中所有实际节点（保留哨兵）
         * 说明：
         * - 在 C# 中不需要手动 delete，但为了更早释放对象并帮助 GC，可以显式断开节点之间的引用：
         *     - 将节点的 Prev 和 Next 置为 null，使这些节点孤立，GC 能更快回收（尤其是大量数据时）。
         * - 最后把 head 恢复成 self-loop 表示空表。
         * - 时间复杂度：O(n)
         */
        public void Clear()
        {
            var cur = head.Next;
            while (cur != head)
            {
                var tmp = cur;
                cur = cur.Next;
                // 断开 tmp 的引用关系，帮助 GC（不是必须，但有益）
                tmp.Prev = null;
                tmp.Next = null;
                // 不需要 delete，GC 会回收 tmp 所占内存（当没有外部引用时）
            }
            head.Next = head;
            head.Prev = head;
            count = 0;
        }
    }

    /*
     * 主程序 - 双向链表使用示例
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("双向链表示例：");
            
            var list = new DoublyLinkedList<int>();
            
            // 添加数据到链表最后
            list.PushBack(10);
            list.PushBack(20);
            list.PushBack(30);
            list.PushBack(40);
            
            Console.WriteLine($"链表数据个数：{list.Count}");
            Console.WriteLine($"头节点数据：{list.FirstNode?.Data}");
            Console.WriteLine($"尾节点数据：{list.LastNode?.Data}");
            
            Console.Write("链表内容：");
            list.Display(); // 期望输出: 10 20 30 40
            
            // 删除指定位置节点（删除索引为1的节点，即20）
            Console.WriteLine($"删除索引1的节点：{list.RemoveAt(1)}");
            
            Console.Write("删除后链表内容：");
            list.Display(); // 期望输出: 10 30 40
            
            Console.WriteLine($"删除后数据个数：{list.Count}");
        }
    }
}

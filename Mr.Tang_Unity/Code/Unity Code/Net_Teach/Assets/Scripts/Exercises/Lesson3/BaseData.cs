using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 所有需要序列化和反序列化数据类的基类，提供自动计算字节数和序列化 已用反射和泛型优化 
/// 继承了BaseData的子类都可以直接抓取public成员变量进行自动序列化和反序列化
/// 嵌套的BaseData子类也不例外，完全不需要写重复的代码了！如果有特殊需求，也可以在子类里 override 重写这两个方法，自定义实现
/// </summary>
public abstract class BaseData
{
    /// <summary>
    /// 获取字节数组容器大小的方法
    /// 使用反射自动计算，子类可直接使用，也可以重写，反射性能一般 若游戏每秒要序列化成千上万个对象，还是建议在子类里 override 重写这两个方法，手动去
    /// </summary>
    /// <returns>字节数组的大小</returns>
    /* =========================================================================================
     * 【第一步：算容量】 获取对象序列化后所占用的字节数组大小。
     * 发送网络数据前，必须先确切知道要给这段数据开辟多大的 byte[] 数组空间。
     * ========================================================================================= */
    public virtual int GetBytesNum()
    {
        int num = 0; // 用于累计所有属性转换成字节后的总长度

        // this.GetType()：获取当前真正在运行的类类型（比如TestInfo）。
        // .GetFields()：【反射API】无视一切，把该类里所有声明为 public 的成员变量全部抓取出来，存入 FieldInfo 数组。
        FieldInfo[] infos = this.GetType().GetFields();

        // 遍历刚才抓取出来的每一个变量
        foreach (FieldInfo info in infos)
        {
            // info.GetValue(this)：获取这个变量在当前对象中（this）实际赋的值。
            // 因为不知道具体是int还是别的啥，所以先装进万能的 object 箱子里。
            object value = info.GetValue(this);

            // info.FieldType：获取这个变量当初声明时的真正类型（比如它是 typeof(int) 还是 typeof(List<string>)）
            Type type = info.FieldType;

            // 【兜底安全机制】：如果用户在这个类里声明了一个嵌套类或者集合，但是忘了 new 出来（值为 null）。
            // 稍后序列化时，如果不分配空间，反序列化对面读取时就会两边字节长度错乱导致整体崩溃。
            if (value == null)
            {
                // IsSubclassOf：判断它是不是 BaseData 的子类（即自己写的自定义类结构）
                // IsGenericType：判读它是不是带有尖括号的泛型集合（如 List<T>）
                if (type.IsSubclassOf(typeof(BaseData)) || 
                   (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(Dictionary<,>))))
                {
                    // 发现为空立刻报警并补救！
                    // Activator.CreateInstance(type)：【反射API】根据类型在内存里动态强制 new 出来一个空实例。
                    value = Activator.CreateInstance(type);
                    // 把刚刚 new 出来的空壳强塞回当前类的这个变量里
                    info.SetValue(this, value);
                }
            }

            // 把类型和值，统一扔给【核心多态类型分发器】去递归计算出这段数据占几个字节，并加入总长度。
            num += CalculateTypeBytes(type, value);
        }
        
        return num; // 最终得到一个准确的总容量
    }

    /* =========================================================================================
     * 【第二步：打包发货 (序列化)】 
     * 将对象里的所有数据，有条不紊地塞进算好大小的 byte[] 字节流里。
     * ========================================================================================= */
    public virtual byte[] Writing()
    {
        int index = 0; // 核心游标！记录当前往字节数组的第几个格子写数据了。
        
        // 按照第一步算好的容量，精准订做用来发货的字节数组。不多不少刚刚好。
        byte[] bytes = new byte[GetBytesNum()];

        // 像刚才一样，用反射再次抓取所有 public 成员变量准备打包。
        FieldInfo[] infos = this.GetType().GetFields();
        foreach (FieldInfo info in infos)
        {
            // 获取当前变量的类型 和 变量的实际值
            // 把它们直接抛给【通用递归解析器 WriteTypeData】。
            // 通过 ref index 把游标的控制权交出去，解析器写几个字节，游标自己就会往前挪几步。
            WriteTypeData(info.FieldType, info.GetValue(this), bytes, ref index);
        }
        
        // 所有数据组装完毕，把这个完整的快递（字节数组）打包返回，丢给 Socket 就能发网上了。
        return bytes;
    }

    /* =========================================================================================
     * 【第三步：拆包拆货 (反序列化)】 
     * 从网络另一端收到了一个纯看不懂的 byte[] 数组，我们要反向把它填入一个空实例里，复原成可用数据。
     * ========================================================================================= */
    public virtual int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex; // 接收网络包同样需要游标，用来记录当前读到哪了。
        
        // 反射抓取：既然我们要给当前这副空壳填数据，那就把它的骨架（成员变量名）全都列出来。
        FieldInfo[] infos = this.GetType().GetFields();

        foreach (FieldInfo info in infos)
        {
             // 调用【通用递归解析器 ReadTypeData】，告诉它：前面那坨字节里，下一个该读一个 info.FieldType 类型的数据了！
             // 解析器会自动从 bytes 字节堆里抠出正确的值返回来。
             object readValue = ReadTypeData(info.FieldType, bytes, ref index);
             
             // info.SetValue(this, value)：【反射API】把解析好的值，强行写进当前对象的这个变量体内。
             info.SetValue(this, readValue);
        }

        // 返回本次总共吃掉了别人发来的多少个字节。（方便后续用来判断这包数据是否处理干净了）
        return index - beginIndex;
    }

    /* ==================================================================================================
                 🔥🔥🔥【核心引警引擎：类型解析与递归分发器模块（支持List和Dict套娃计算）】🔥🔥🔥
       这下面三个长长的方法，负责鉴别（int, float, List 等等）形形色色的具体类型，
       只要遇到了泛型集合或自定义嵌套类，它们就会机智地【调用自己（递归）】，实现无限套娃的解析能力。
     ==================================================================================================*/

    /// <summary>
    /// 【通用的容量计算器】
    /// 无论外面喂进来的是基础类型、集合还是自定义类，这里统统计算出它的字节大小。
    /// </summary>
    private int CalculateTypeBytes(Type type, object value)
    {
        // 基础的值类型（也就是占用内存大小固定的那帮兄弟），直接用 sizeof() 秒算：
        if (type == typeof(int)) return sizeof(int);
        if (type == typeof(long)) return sizeof(long);
        if (type == typeof(short)) return sizeof(short);
        if (type == typeof(float)) return sizeof(float);
        if (type == typeof(byte)) return sizeof(byte);
        if (type == typeof(bool)) return sizeof(bool);
        // 字符串(string)：它的长度是不固定的（叫可变长度数据）。
        // 在网络传输时我们有一个固定规矩：先头传一个 int 代表字符串有几个字，然后才是内容本体。
        if (type == typeof(string)) 
            // 长度 = (装字数用的 4个字节) + (字符串转化成UTF8格式后的实际字长)
            return value == null ? sizeof(int) : sizeof(int) + Encoding.UTF8.GetByteCount((string)value);
        // 如果是个嵌套在一起的自定义类。就直接调那个类的 GetBytesNum 算！
        if (type.IsSubclassOf(typeof(BaseData))) 
            return value == null ? 0 : ((BaseData)value).GetBytesNum();

        // 【集合解析策略一：List 列表】
        // 条件判断：如果是泛型 且 它的本体是 List 的话
        // ⭐【集合List的逆向提权解析】：当看到它是 List 集合
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            // 打包 List 时有一个必须遵守的定式规矩：先写入一个 int (4个字节)，代表这个 List 里面装了几件货。
            int num = sizeof(int);
            if (value != null)
            {
                IList list = value as IList; // 用底层的 IList 接口统管所有集合
                // .GetGenericArguments()[0]：【反射API】专门用来扒开 List<T> 的衣服，看看那个 T 到底是个啥长相（Type）。
                // 确认一下当时发过来的货是啥材质（拿 T）
            Type itemType = type.GetGenericArguments()[0];
                foreach (var item in list)
                    num += CalculateTypeBytes(itemType, item); // 递归魔法开启！把里面装的具体值（哪怕它是一个疯狂套娃的三维数组），重新丢回到自己头顶计算大小！
            }
            return num;
        }

        // 【集合解析策略二：Dictionary 字典】
        // 当匹配到泛型且是 Dictionary<,> 时
        // ⭐【集合Dictionary的解盘】：
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            // 规矩一样：字典头上必须贴一个 int 标签来代表有几组键值对存放。
            int num = sizeof(int);
            if (value != null)
            {
                IDictionary dict = value as IDictionary; // 用底层接口统管一切泛型字典
                // 扒开 <K, V> 的衣服：[0]是 Key的Type，[1]是 Value的Type。
                Type keyType = type.GetGenericArguments()[0];
                Type valType = type.GetGenericArguments()[1]; // 获取<V>类型
                // 遍历字典，把那对双胞胎（Key 和 Value）分开丢进计算器去算大小。
                foreach (DictionaryEntry entry in dict)
                {
                    num += CalculateTypeBytes(keyType, entry.Key);
                    num += CalculateTypeBytes(valType, entry.Value);
                }
            }
            return num;
        }

        return 0; // 其他暂不支持的类型
    }

    private void WriteTypeData(Type type, object value, byte[] bytes, ref int index)
    {
        if (type == typeof(int)) WriteInt(bytes, value == null ? 0 : (int)value, ref index);
        else if (type == typeof(long)) WriteLong(bytes, value == null ? 0L : (long)value, ref index);
        else if (type == typeof(short)) WriteShort(bytes, value == null ? (short)0 : (short)value, ref index);
        else if (type == typeof(float)) WriteFloat(bytes, value == null ? 0f : (float)value, ref index);
        else if (type == typeof(byte)) WriteByte(bytes, value == null ? (byte)0 : (byte)value, ref index);
        else if (type == typeof(bool)) WriteBool(bytes, value == null ? false : (bool)value, ref index);
        else if (type == typeof(string)) WriteString(bytes, value == null ? "" : (string)value, ref index);
        else if (type.IsSubclassOf(typeof(BaseData)))
        {
            if (value != null) WriteData(bytes, (BaseData)value, ref index);
        }
        else // ⭐【集合List的逆向提权解析】：当看到它是 List 集合
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            IList list = value as IList; // 用底层的 IList 接口统管所有集合
            if (list == null) WriteInt(bytes, 0, ref index);
            else
            {
                WriteInt(bytes, list.Count, ref index); // 头：写数量
                // 确认一下当时发过来的货是啥材质（拿 T）
            Type itemType = type.GetGenericArguments()[0];
                foreach (var item in list) 
                    WriteTypeData(itemType, item, bytes, ref index); // 身：递归去写里面的每个元素！
            }
        }
        else // ⭐【集合Dictionary的解盘】：
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            IDictionary dict = value as IDictionary; // 用底层接口统管一切泛型字典
            if (dict == null) WriteInt(bytes, 0, ref index);
            else
            {
                WriteInt(bytes, dict.Count, ref index); // 写字典大小
                Type keyType = type.GetGenericArguments()[0];
                Type valType = type.GetGenericArguments()[1];
                // 遍历字典，把那对双胞胎（Key 和 Value）分开丢进计算器去算大小。
                foreach (DictionaryEntry entry in dict)
                {
                    WriteTypeData(keyType, entry.Key, bytes, ref index); // 写键
                    WriteTypeData(valType, entry.Value, bytes, ref index); // 写值
                }
            }
        }
    }

    /// <summary>
    /// 【通用的万金油读取器（拆包）】
    /// 根据给定的类型（Type），它能在长长的全是一堆杂乱数字的 byte[] 组里，精准裁下对应的一块，还原成想要的东西并返回出去
    /// </summary>
    private object ReadTypeData(Type type, byte[] bytes, ref int index)
    {
        // 发现是原模原样的基础值大队，就外包给底层的脏活苦力老大哥帮忙去内存里用转换器剥茧抽丝：
        if (type == typeof(int)) return ReadInt(bytes, ref index);
        if (type == typeof(long)) return ReadLong(bytes, ref index);
        if (type == typeof(short)) return ReadShort(bytes, ref index);
        if (type == typeof(float)) return ReadFloat(bytes, ref index);
        if (type == typeof(byte)) return ReadByte(bytes, ref index);
        if (type == typeof(bool)) return ReadBool(bytes, ref index);
        if (type == typeof(string)) return ReadString(bytes, ref index);
        // 如果属于复杂的嵌套基类兄弟，委托另外一个兄弟帮它解盘
        if (type.IsSubclassOf(typeof(BaseData))) return ReadData(type, bytes, ref index);

        // ⭐【集合List的逆向提权解析】：当看到它是 List 集合
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            // 第1步：我们序列化时写在了开头。所以这里必须要先读取出那个存着数量的 int，这决定了后续我们要循环拆几个包裹。
            int count = ReadInt(bytes, ref index);
            // 没有包裹直接交一个空箱子回去
            if (count == 0) return Activator.CreateInstance(type);

            // 第2步：当知道对面发了多少包裹后，咱们这边必须建个一模一样的空货箱，准备接货了。
            IList list = Activator.CreateInstance(type) as IList;
            // 确认一下当时发过来的货是啥材质（拿 T）
            Type itemType = type.GetGenericArguments()[0];
            for (int i = 0; i < count; i++) // 第3步：开始按照表头的循环数不断接力拆箱
            {
                // 把刚从字节里剥离和解读出来的数据，塞入大货箱(List 列表)中！
                list.Add(ReadTypeData(itemType, bytes, ref index));
            }
            // 返回装满数据的完美宝箱
            return list;
        }

        // ⭐【集合Dictionary的解盘】：
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            int count = ReadInt(bytes, ref index);
            // 没有包裹直接交一个空箱子回去
            if (count == 0) return Activator.CreateInstance(type);

            IDictionary dict = Activator.CreateInstance(type) as IDictionary;
            Type keyType = type.GetGenericArguments()[0];
            Type valType = type.GetGenericArguments()[1];
            // 开始对装满了各种 Key 和 Value 对应数据的字典库下手提取
            for (int i = 0; i < count; i++)
            {
                // 解一个 Key
                object key = ReadTypeData(keyType, bytes, ref index);
                // 解它隔壁挨着的 Value
                object val = ReadTypeData(valType, bytes, ref index);
                // 放到字典大组中拼接
                dict.Add(key, val);
            }
            return dict;
        }

        return null;
    }
    // ==========================================
    /* ==================================================================================================
                 🔧🔧🔧【脏活累活工厂区：基础底层解析库（被上面代码核心当包工头调遣的）】🔧🔧🔧
         使用 C# 的 BitConverter 这个将高级语言数据 翻译成 计算机底层二进制的核心转换神器进行工作。
     ==================================================================================================*/

    // 下方为具体类型的辅助写入方法 
    // ==========================================

    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        // BitConverter.GetBytes(value)：把一串数字（比如 9999）拍成 4 个 byte 碎片
        // .CopyTo(bytes, index)：把排好的碎片放进咱发货用的 bytes 里面，并且是从指定的 index (游标) 这个坑位往里面顺次挤。
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        
        // 游标自己必须往前移动：塞了一个 int 进去了对吧，游标往右平移 4 个坑位。（sizeof(int)是算大小必学指令）
        index += sizeof(int);
    }

    protected void WriteLong(byte[] bytes, long value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(long);
    }

    protected void WriteShort(byte[] bytes, short value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(short);
    }

    protected void WriteFloat(byte[] bytes, float value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(float);
    }

    protected void WriteByte(byte[] bytes, byte value, ref int index)
    {
        bytes[index] = value;
        index += sizeof(byte);
    }

    protected void WriteBool(byte[] bytes, bool value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(bool);
    }

    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        // Encoding.UTF8.GetBytes：把中文和英文统统按世界最通用标准 UTF8 碾碎成了字节！
        byte[] strBytes = Encoding.UTF8.GetBytes(value);
        // 铁律：写内容前先告诉对面咱们这个字符串到底多大，不然网络发出去人家不知道啥时候是个头。
        WriteInt(bytes, strBytes.Length, ref index);
        // 字节复印机启动
        strBytes.CopyTo(bytes, index); 
        
        // 内容长度加上即可
        index += strBytes.Length;
    }

    protected void WriteData(byte[] bytes, BaseData data, ref int index)
    {
        // 如果碰到了一个活着的继承BaseData结构的家伙。干脆放权让他自己调用 Writing去。这很 OOP (面向对象)
        data.Writing().CopyTo(bytes, index);
        index += data.GetBytesNum();
    }



    // ==========================================
    // 逆向反转组区：从长条形的字节流里，切猪肉一样往下一点点取值并还原
    // ==========================================

    protected int ReadInt(byte[] bytes, ref int index)
    {
        // BitConverter.ToInt32：去这堆肉里(bytes)，从指定的下刀处(index游标处) 切 4段字节并当成数字还原翻译回 C#的世界
        int value = BitConverter.ToInt32(bytes, index);
        index += sizeof(int); // 切走了 4 块，游标往右挪继续等下一次切片
        return value;
    }

    protected long ReadLong(byte[] bytes, ref int index)
    {
        long value = BitConverter.ToInt64(bytes, index);
        index += sizeof(long);
        return value;
    }

    protected short ReadShort(byte[] bytes, ref int index)
    {
        short value = BitConverter.ToInt16(bytes, index);
        index += sizeof(short);
        return value;
    }

    protected float ReadFloat(byte[] bytes, ref int index)
    {
        float value = BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        return value;
    }

    protected byte ReadByte(byte[] bytes, ref int index)
    {
        byte value = bytes[index];
        index += sizeof(byte);
        return value;
    }

    protected bool ReadBool(byte[] bytes, ref int index)
    {
        bool value = BitConverter.ToBoolean(bytes, index);
        index += sizeof(bool);
        return value;
    }

    protected string ReadString(byte[] bytes, ref int index)
    {
        // 逆向解盘字符串，既然咱们存的时候先把它的表头容量写第一的位置。咱这里读的第一眼自然碰到的必须它！
        int length = ReadInt(bytes, ref index); 
        
        // 既然拿到了真正文字占几块肉，接着用世界标准切过去还原。
        string str = Encoding.UTF8.GetString(bytes, index, length);
        
        index += length;
        return str;
    }

    protected BaseData ReadData(Type type, byte[] bytes, ref int index)
    {
        // 【多态精髓】原理和兜底初始化一样：Activator.CreateInstance(type) 根据类型信息在内存中动态强行new一个实例出来，并用 as 安全转为 BaseData 基类引用
        BaseData data = Activator.CreateInstance(type) as BaseData; 
        
        // 转成 BaseData 后，就能利用多态去调用该子类继承来的 Reading 方法，让它自己解析自己的字节流内容
        index += data.Reading(bytes, index); 
        
        return data; // 返回填满了丰满血肉的新鲜实例数据。
    }
}

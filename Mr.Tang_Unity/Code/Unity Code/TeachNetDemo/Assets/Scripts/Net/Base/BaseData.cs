using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 协议数据基类。所有需要序列化/反序列化的数据结构类都继承此类。
/// 通过反射自动收集子类的 public 实例字段，完成：算容量 → 序列化 → 反序列化 三步流程。
/// 支持的字段类型：int, long, short, float, byte, bool, string, enum, BaseData子类, 数组T[], List&lt;T&gt;, Dictionary&lt;K,V&gt;（可嵌套）。
/// 如果子类有特殊序列化需求，可以 override GetBytesNum / Writing / Reading。
/// </summary>
public abstract class BaseData
{
    // 控制字段收集范围：只收集 public 实例字段。后续如果要支持 private + 标记，只改这一处和 GetSerializableFields。
    private const BindingFlags SERIALIZE_FIELD_FLAGS = BindingFlags.Instance | BindingFlags.Public;

    /// <summary>
    /// 获取当前子类中所有参与序列化的字段。
    /// 三步流程（算容量、写入、读取）统一走这个入口，保证字段列表和顺序完全一致，
    /// 避免三处各自调用 GetFields 导致规则不同步。
    /// </summary>
    private FieldInfo[] GetSerializableFields() // 获取 可序列化 字段
    {
        return this.GetType().GetFields(SERIALIZE_FIELD_FLAGS);
    }

    /// <summary>
    /// 空值兜底：如果某个复合类型字段（BaseData子类 / 数组 / List / Dictionary）为 null，
    /// 自动创建一个空实例并回写到字段上。
    /// 这样序列化时能输出"长度=0"的合法数据，反序列化端能正确读到空集合，
    /// 不会因为写入端跳过该字段而导致读取时游标错位崩溃。
    /// </summary>
    /// <param name="info">字段的反射信息，用于把新建的空实例写回对象</param>
    /// <param name="type">字段声明类型</param>
    /// <param name="value">当前字段值（调用前已确认为 null）</param>
    /// <returns>新创建的空实例，或原值（如果该类型不需要兜底）</returns>
    private object EnsureAutoCreatedValue(FieldInfo info, Type type, object value)
    {
        if (value != null) // 已经有值了就不需要兜底，直接返回原值
            return value; // 返回原值，调用方继续正常计算字节数或写入数据

        if (type.IsSubclassOf(typeof(BaseData)) || // BaseData 子类用 Activator.CreateInstance 创建空实例；数组/List/Dictionary 用 Array.CreateInstance 或 Activator.CreateInstance 创建空实例
            type.IsArray || // 数组类型需要兜底
            (type.IsGenericType && // 泛型类型需要进一步判断是否是 List<> 或 Dictionary<,>，因为只有这两种集合类型需要兜底补空实例；其他泛型类型不处理
            (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))) // 数组/List/Dictionary 类型需要兜底
        {
            // 数组用 Array.CreateInstance 创建长度为0的空数组；其余用 Activator.CreateInstance 调无参构造
            object instance = type.IsArray
                ? Array.CreateInstance(type.GetElementType(), 0) // 创建长度为0的空数组
                : Activator.CreateInstance(type); // 创建 List<T> 或 Dictionary<K,V> 的空实例

            info.SetValue(this, instance); // 把新建的空实例写回当前对象的该字段，确保后续序列化能正确输出"长度=0"的数据
            return instance; // 返回新建的空实例，调用方继续正常计算字节数或写入数据
        }

        return value; // 其他类型不需要兜底，直接返回原值（虽然按当前逻辑应该都是 null，但保持一致性）供调用方继续正常计算字节数或写入数据
    }

    /// <summary>
    /// 获取枚举的底层整型类型，C# 的枚举本质上是“某种整型的别名”
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <returns>枚举的底层整型类型</returns>
    private Type GetEnumUnderlyingType(Type enumType) // 获取枚举的底层整型类型
    {
        return Enum.GetUnderlyingType(enumType); // C# 的枚举本质上是“某种整型的别名”，默认是 int，但也可以指定为 byte、short、long 等；
                                                // 这个方法能正确获取到声明的底层类型，确保后续计算字节数和读写流程都能正确处理枚举字段
    }

    /// <summary>
    /// 获取枚举值的底层整型值。枚举字段在协议中按其底层整型类型序列化，所以这里需要把枚举实例转换成对应的整型值。
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="enumValue">枚举实例</param>
    /// <returns>枚举值的底层整型值</returns>
    private object GetEnumUnderlyingValue(Type enumType, object enumValue) // 获取枚举值的底层整型值
    {
        Type underlyingType = GetEnumUnderlyingType(enumType); // 获取枚举的底层整型类型
        object safeValue = enumValue ?? Activator.CreateInstance(enumType); // 如果枚举值为 null（虽然按当前逻辑应该不会，但这里做个兜底），就创建一个默认实例（对应底层整型的默认值，如 int 的 0），确保后续转换不会出错
        return Convert.ChangeType(safeValue, underlyingType); // 把枚举实例转换成对应的底层整型值，供后续按底层类型计算字节数和写入数据；Convert.ChangeType 能正确处理各种底层类型的转换，如 int、byte、short、long 等，确保枚举字段在协议中能正确序列化为其底层整型表示
    }

    /// <summary>
    /// 先按底层整型读出值，再还原成目标枚举实例。
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="bytes">字节数组</param>
    /// <param name="index">当前读取索引</param>
    /// <returns>还原后的枚举实例</returns>
    private object ReadEnumValue(Type enumType, byte[] bytes, ref int index) // 先按底层整型读出值，再还原成目标枚举实例
    {
        Type underlyingType = GetEnumUnderlyingType(enumType); // 获取枚举的底层整型类型
        object underlyingValue = ReadTypeData(underlyingType, bytes, ref index); // 按底层整型类型从字节数组中读取值
        return Enum.ToObject(enumType, underlyingValue); // 将底层整型值还原为枚举实例
    }

    #region 三步流程：GetBytesNum → Writing → Reading

    /// <summary>
    /// 【第一步：算容量】计算当前对象序列化后占用的总字节数。
    /// 发送前必须先知道 byte[] 要开多大，所以这一步在 Writing 之前调用。
    /// 内部会对 null 的复合字段做兜底补空（EnsureAutoCreatedValue），确保后续序列化不会因 null 跳过字段。
    /// 子类可直接使用；如果每秒要序列化大量对象，也可以 override 手写以避开反射开销。
    /// </summary>
    /// <returns>序列化后的总字节数</returns>
    public virtual int GetBytesNum()
    {
        int num = 0;
        FieldInfo[] infos = GetSerializableFields();

        foreach (FieldInfo info in infos)
        {
            object value = info.GetValue(this);  // 取出该字段在当前实例中的值
            Type type = info.FieldType;           // 获取字段声明类型，用于后续的类型分发

            // 复合类型为 null 时补一个空实例，确保序列化时能写出合法的"长度=0"数据
            if (value == null)
                value = EnsureAutoCreatedValue(info, type, value);

            num += CalculateTypeBytes(type, value); // 按类型递归计算该字段占多少字节
        }

        return num;
    }

    /// <summary>
    /// 【第二步：序列化】把当前对象的所有字段按声明顺序写入 byte[] 并返回。
    /// 内部先调用 GetBytesNum() 确定数组大小（同时完成 null 兜底），再逐字段写入。
    /// </summary>
    /// <returns>完整的序列化字节数组，可直接通过 Socket 发送</returns>
    public virtual byte[] Writing()
    {
        int index = 0;                          // 写入游标：记录当前写到 byte[] 的第几个位置
        byte[] bytes = new byte[GetBytesNum()]; // GetBytesNum 已完成 null 兜底，这里拿到精确容量

        FieldInfo[] infos = GetSerializableFields();
        foreach (FieldInfo info in infos)
            WriteTypeData(info.FieldType, info.GetValue(this), bytes, ref index);

        return bytes;
    }

    /// <summary>
    /// 【第三步：反序列化】从 byte[] 中按字段声明顺序依次读取值，填入当前对象的各个字段。
    /// </summary>
    /// <param name="bytes">收到的原始字节数组</param>
    /// <param name="beginIndex">本对象数据在 bytes 中的起始偏移</param>
    /// <returns>本次读取消耗的字节数（调用方可据此移动自己的游标）</returns>
    public virtual int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex; // 读取游标
        FieldInfo[] infos = GetSerializableFields();

        foreach (FieldInfo info in infos)
        {
            object readValue = ReadTypeData(info.FieldType, bytes, ref index); // 按类型从 bytes 中读出值
            info.SetValue(this, readValue); // 通过反射将读出的值写入当前对象的该字段
        }

        return index - beginIndex; // 返回消耗的字节数，外层可据此推进自己的游标
    }

    #endregion

    #region 类型分发器：CalculateTypeBytes / WriteTypeData / ReadTypeData

    // 下面三个方法根据字段的具体类型分派到对应的处理逻辑。
    // 遇到集合或 BaseData 子类时会递归调用自身，支持任意深度的嵌套结构。

    /// <summary>
    /// 根据类型计算单个值序列化后占多少字节。
    /// 基础值类型直接 sizeof；string 前置 4 字节长度头 + UTF8 内容；
    /// 数组/List/Dictionary 前置 4 字节表示元素数量，再累加每个元素。
    /// </summary>
    /// <param name="type">字段声明类型</param>
    /// <param name="value">字段当前值，用于计算可变长度（如 string、集合）</param>
    /// <returns>该值序列化后占的字节数</returns>
    private int CalculateTypeBytes(Type type, object value)
    {
        // --- 基础定长类型 ---
        if (type == typeof(int))    return sizeof(int);
        if (type == typeof(long))   return sizeof(long);
        if (type == typeof(short))  return sizeof(short);
        if (type == typeof(float))  return sizeof(float);
        if (type == typeof(byte))   return sizeof(byte);
        if (type == typeof(bool))   return sizeof(bool);

        // enum 底层本质仍是整型，字节数跟它声明的底层类型保持一致
        if (type.IsEnum)
            return CalculateTypeBytes(GetEnumUnderlyingType(type), GetEnumUnderlyingValue(type, value));

        // string 是可变长度类型，协议格式：[4字节:UTF8字节长度] + [UTF8内容]
        if (type == typeof(string))
            return sizeof(int) + (value == null ? 0 : Encoding.UTF8.GetByteCount((string)value));

        // BaseData 子类：委托该实例自身的 GetBytesNum() 递归计算
        if (type.IsSubclassOf(typeof(BaseData)))
            return value == null ? 0 : ((BaseData)value).GetBytesNum();

        // --- 数组 T[] ---
        // 协议格式：[4字节:元素数量] + [逐个元素的字节]
        if (type.IsArray)
        {
            int num = sizeof(int); // 元素数量头
            if (value != null)
            {
                Array array = (Array)value;
                Type elementType = type.GetElementType(); // 获取数组元素类型，如 int[] → int
                foreach (var item in array)
                    num += CalculateTypeBytes(elementType, item);
            }
            return num;
        }

        // --- List<T> ---
        // 协议格式与数组相同：[4字节:元素数量] + [逐个元素的字节]
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            int num = sizeof(int);
            if (value != null)
            {
                IList list = (IList)value; // 用非泛型 IList 接口统一操作任意 List<T>
                Type itemType = type.GetGenericArguments()[0]; // 获取泛型参数 T 的实际类型
                foreach (var item in list)
                    num += CalculateTypeBytes(itemType, item);
            }
            return num;
        }

        // --- Dictionary<K,V> ---
        // 协议格式：[4字节:键值对数量] + 逐对 [Key字节 + Value字节]
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            int num = sizeof(int);
            if (value != null)
            {
                IDictionary dict = (IDictionary)value; // 非泛型 IDictionary 统一操作任意 Dictionary<K,V>
                Type keyType = type.GetGenericArguments()[0]; // K 的类型
                Type valType = type.GetGenericArguments()[1]; // V 的类型
                foreach (DictionaryEntry entry in dict)
                {
                    num += CalculateTypeBytes(keyType, entry.Key);
                    num += CalculateTypeBytes(valType, entry.Value);
                }
            }
            return num;
        }

        return 0; // 未支持的类型不占字节（后续扩展时在这里加分支）
    }

    /// <summary>
    /// 根据类型把 value 写入 bytes[index] 位置，写完后 index 自动后移。
    /// 类型分支顺序与 CalculateTypeBytes 保持一致，确保写入字节数和预算容量完全匹配。
    /// </summary>
    private void WriteTypeData(Type type, object value, byte[] bytes, ref int index)
    {
        // --- 基础类型：null 时写默认值，保证字节对齐 ---
        if      (type == typeof(int))    WriteInt(bytes, value == null ? 0 : (int)value, ref index);
        else if (type == typeof(long))   WriteLong(bytes, value == null ? 0L : (long)value, ref index);
        else if (type == typeof(short))  WriteShort(bytes, value == null ? (short)0 : (short)value, ref index);
        else if (type == typeof(float))  WriteFloat(bytes, value == null ? 0f : (float)value, ref index);
        else if (type == typeof(byte))   WriteByte(bytes, value == null ? (byte)0 : (byte)value, ref index);
        else if (type == typeof(bool))   WriteBool(bytes, value == null ? false : (bool)value, ref index);
        else if (type.IsEnum)            WriteTypeData(GetEnumUnderlyingType(type), GetEnumUnderlyingValue(type, value), bytes, ref index);
        else if (type == typeof(string)) WriteString(bytes, value == null ? "" : (string)value, ref index);

        // --- BaseData 子类：委托子类自身的 Writing() 输出（GetBytesNum 已兜底，正常不会为 null） ---
        else if (type.IsSubclassOf(typeof(BaseData)))
        {
            if (value != null) WriteData(bytes, (BaseData)value, ref index);
        }

        // --- 数组 T[] ---
        else if (type.IsArray)
        {
            Array array = value as Array;
            if (array == null) { WriteInt(bytes, 0, ref index); return; }

            WriteInt(bytes, array.Length, ref index); // 先写元素数量
            Type elementType = type.GetElementType();
            foreach (var item in array)
                WriteTypeData(elementType, item, bytes, ref index); // 逐个递归写入
        }

        // --- List<T> ---
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            IList list = value as IList;
            if (list == null) { WriteInt(bytes, 0, ref index); return; }

            WriteInt(bytes, list.Count, ref index);
            Type itemType = type.GetGenericArguments()[0];
            foreach (var item in list)
                WriteTypeData(itemType, item, bytes, ref index);
        }

        // --- Dictionary<K,V> ---
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            IDictionary dict = value as IDictionary;
            if (dict == null) { WriteInt(bytes, 0, ref index); return; }

            WriteInt(bytes, dict.Count, ref index);
            Type keyType = type.GetGenericArguments()[0];
            Type valType = type.GetGenericArguments()[1];
            foreach (DictionaryEntry entry in dict)
            {
                WriteTypeData(keyType, entry.Key, bytes, ref index);
                WriteTypeData(valType, entry.Value, bytes, ref index);
            }
        }
    }

    /// <summary>
    /// 根据类型从 bytes[index] 位置读取一个值并返回，读完后 index 自动后移。
    /// 与 WriteTypeData 的写入顺序严格对称：写什么顺序，这里就读什么顺序。
    /// </summary>
    private object ReadTypeData(Type type, byte[] bytes, ref int index)
    {
        // --- 基础类型 ---
        if (type == typeof(int))    return ReadInt(bytes, ref index);
        if (type == typeof(long))   return ReadLong(bytes, ref index);
        if (type == typeof(short))  return ReadShort(bytes, ref index);
        if (type == typeof(float))  return ReadFloat(bytes, ref index);
        if (type == typeof(byte))   return ReadByte(bytes, ref index);
        if (type == typeof(bool))   return ReadBool(bytes, ref index);
        if (type.IsEnum)            return ReadEnumValue(type, bytes, ref index);
        if (type == typeof(string)) return ReadString(bytes, ref index);

        // --- BaseData 子类：通过反射创建空实例，再调其 Reading 让它自己填充数据 ---
        if (type.IsSubclassOf(typeof(BaseData))) return ReadData(type, bytes, ref index);

        // --- 数组 T[]：先读元素数量，再创建同长度数组，逐个读取并填入 ---
        if (type.IsArray)
        {
            int count = ReadInt(bytes, ref index);
            Type elementType = type.GetElementType();
            Array array = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
                array.SetValue(ReadTypeData(elementType, bytes, ref index), i);
            return array;
        }

        // --- List<T>：先读元素数量，再逐个读取并 Add 到新建的 List 中 ---
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            int count = ReadInt(bytes, ref index);
            IList list = (IList)Activator.CreateInstance(type); // 等价于 new List<T>()
            Type itemType = type.GetGenericArguments()[0];
            for (int i = 0; i < count; i++)
                list.Add(ReadTypeData(itemType, bytes, ref index));
            return list;
        }

        // --- Dictionary<K,V>：先读键值对数量，再逐对读取 key、value 并 Add ---
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            int count = ReadInt(bytes, ref index);
            IDictionary dict = (IDictionary)Activator.CreateInstance(type); // 等价于 new Dictionary<K,V>()
            Type keyType = type.GetGenericArguments()[0];
            Type valType = type.GetGenericArguments()[1];
            for (int i = 0; i < count; i++)
            {
                object key = ReadTypeData(keyType, bytes, ref index);
                object val = ReadTypeData(valType, bytes, ref index);
                dict.Add(key, val);
            }
            return dict;
        }

        return null;
    }

    #endregion
    
    #region 底层读写工具方法

    // 下面是最终执行 BitConverter 转换的辅助方法。
    // 写入方法：把值转成 byte[] 后复制到目标数组的 index 位置，然后 index 后移该类型的固定长度。
    // 读取方法：从 byte[] 的 index 位置按类型长度取值，然后 index 后移。

    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
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
        bytes[index] = value; // byte 只有 1 字节，直接赋值
        index += sizeof(byte);
    }

    protected void WriteBool(byte[] bytes, bool value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(bool);
    }

    /// <summary>
    /// 写入 string：先写 4 字节的 UTF8 字节长度，再写 UTF8 编码后的内容。
    /// 读取端按同样规则先读长度再读内容，就能正确还原可变长度的字符串。
    /// </summary>
    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        byte[] strBytes = Encoding.UTF8.GetBytes(value);
        WriteInt(bytes, strBytes.Length, ref index); // 先写长度头
        strBytes.CopyTo(bytes, index);              // 再写字符串内容本体
        index += strBytes.Length;
    }

    /// <summary>
    /// 写入 BaseData 子类：调用该子类的 Writing() 获取其完整序列化结果，再整段复制到目标数组中。
    /// 用 dataBytes.Length 推进游标，避免再调一次 GetBytesNum() 导致重复反射开销。
    /// </summary>
    protected void WriteData(byte[] bytes, BaseData data, ref int index)
    {
        byte[] dataBytes = data.Writing();
        dataBytes.CopyTo(bytes, index);
        index += dataBytes.Length;
    }

    // --- 读取方法：从 bytes[index] 开始按类型长度取值，index 自动后移 ---

    protected int ReadInt(byte[] bytes, ref int index)
    {
        int value = BitConverter.ToInt32(bytes, index);
        index += sizeof(int);
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

    /// <summary>
    /// 读取 string：先读 4 字节长度头，再按该长度从 bytes 中取出 UTF8 字节并解码为字符串。
    /// </summary>
    protected string ReadString(byte[] bytes, ref int index)
    {
        int length = ReadInt(bytes, ref index); // 读出 UTF8 内容的字节长度
        string str = Encoding.UTF8.GetString(bytes, index, length);
        index += length;
        return str;
    }

    /// <summary>
    /// 读取 BaseData 子类：通过 Activator 反射创建该子类的空实例，然后调用其 Reading() 填充数据。
    /// Reading() 返回消耗的字节数，这里累加到 index 上以推进游标。
    /// </summary>
    /// <param name="type">子类的实际 Type（如 typeof(PlayerData)），用于动态创建正确的实例</param>
    protected BaseData ReadData(Type type, byte[] bytes, ref int index)
    {
        BaseData data = Activator.CreateInstance(type) as BaseData;
        index += data.Reading(bytes, index);
        return data;
    }

    #endregion
}

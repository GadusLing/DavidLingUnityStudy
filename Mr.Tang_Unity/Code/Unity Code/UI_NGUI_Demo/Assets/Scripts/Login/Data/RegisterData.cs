using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注册数据
/// </summary>
public class RegisterData
{
    // 用了之前我们写XML时写的自定义序列化字典，通过这个类可以用XML接口直接保存字典，字典中key是用户名，value是密码，字典元素数表示用户数量
    public SerializerDictionary<string, string> registerInfo = new SerializerDictionary<string, string>();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterData
{
    /// <summary>
    /// 一个字典，键为用户名，值为密码，可以通过RegisterData["username"]获取对应的密码信息
    /// </summary>
    public Dictionary<string, string> registerInfo = new Dictionary<string, string>();

}

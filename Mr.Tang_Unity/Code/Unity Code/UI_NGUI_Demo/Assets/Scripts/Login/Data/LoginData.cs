using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LoginData
{
    //用户名
    public string username;
    //密码
    public string password;
    // 上次登录的服务器ID
    public string frontServerID;
    // 是否记住密码和自动登录
    public bool rememberPassword;
    public bool autoLogin;

}

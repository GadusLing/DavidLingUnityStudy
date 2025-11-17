using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登陆界面需要记录的玩家操作相关的数据
/// </summary>
public class LoginData
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string username;
    /// <summary>
    /// 密码
    /// </summary>
    public string password;
    /// <summary>
    /// 记住密码
    /// </summary>
    public bool rememberMe;
    /// <summary>
    /// 自动登录
    /// </summary>
    public bool autoLogin;
    /// <summary>
    /// 上次登录的服务器ID -1代表没有选择过服务器
    /// </summary>
    public int frontServerId;

}

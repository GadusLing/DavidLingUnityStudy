using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登录管理器 主要用于管理登录相关的数据
/// </summary>
public class LoginMgr 
{
    private static LoginMgr instance = new LoginMgr();
    public static LoginMgr Instance => instance;

    private LoginMgr() 
    { 
        loginData = XmlDataMgr.Instance.LoadData(typeof(LoginData), "LoginData") as LoginData;
        registerData = XmlDataMgr.Instance.LoadData(typeof(RegisterData), "RegisterData") as RegisterData;
        serverInfo = XmlDataMgr.Instance.LoadData(typeof(ServerInfo), "ServerInfo") as ServerInfo;
    }

    public bool checkInfo(string username, string password)
    {
        //检查玩家用户名密码，简单的本地XML信息验证逻辑，实际项目中需要与服务器交互
        if (registerData.registerInfo.ContainsKey(username) && registerData.registerInfo[username] == password)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 登录面板上的数据 例：保存密码 自动登录
    /// </summary>
    private LoginData loginData;
    //提供给外部获取数据的属性，属性只能读不能写，避免登录信息被外部随意修改
    public LoginData LoginData => loginData;
    //提供给外部保存数据的方法
    public void SaveLoginData()
    {
        XmlDataMgr.Instance.SaveData(loginData, "LoginData");
    }

    /// <summary>
    /// 服务器数据 例：所有服务器的服务器名 状态 是否新服
    /// </summary>
    private ServerInfo serverInfo;
    public ServerInfo ServerInfo => serverInfo;

    /// <summary>
    /// 注册数据 例：1个用户名对应1个密码
    /// </summary>
    private RegisterData registerData;
    public RegisterData RegisterData => registerData;
    public void SaveRegisterData()
    {
        XmlDataMgr.Instance.SaveData(registerData, "RegisterData");
    }
    //提供给外部使用的注册方法，返回值判断是否注册成功
    public bool RegisterUser(string username, string password)
    {
        //简单的本地XML信息注册逻辑，实际项目中需要与服务器交互
        if (registerData.registerInfo.ContainsKey(username))
        {
            return false;
        }
        registerData.registerInfo.Add(username, password);
        SaveRegisterData();
        return true;
    }

}

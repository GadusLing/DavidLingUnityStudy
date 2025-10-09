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
    }

    private LoginData loginData;

    //提供给外部获取数据的属性，属性只能读不能写，避免登录信息被外部随意修改
    public LoginData LoginData => loginData;

    //提供给外部保存数据的方法
    public void SaveLoginData()
    {
        XmlDataMgr.Instance.SaveData(loginData, "LoginData");
    }

}

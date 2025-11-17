using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoginMgr
{
    private static LoginMgr _instance = new LoginMgr();
    public static LoginMgr Instance => _instance;

    
    private LoginData loginData = new LoginData();
    /// <summary>
    /// 登录数据
    /// </summary>
    public LoginData LoginData => loginData;

    
    private RegisterData registerData = new RegisterData();
    /// <summary>
    /// 注册数据
    /// </summary>
    public RegisterData RegisterData => registerData;

    private List<ServerInfo> serverData;
    /// <summary>
    /// 服务器名称状态数据
    /// </summary>
    public List<ServerInfo> ServerData => serverData;

    



    private LoginMgr()
    {
        // 通过json管理器加载登录数据
        loginData = JsonMgr.Instance.LoadData<LoginData>("LoginData");
        // 加载注册数据
        registerData = JsonMgr.Instance.LoadData<RegisterData>("RegisterData"); 
        // 读取服务器名称状态数据
        serverData = JsonMgr.Instance.LoadData<List<ServerInfo>>("ServerInfo");

    }

    /// <summary>
    /// 保存登录数据
    /// </summary>
    public void SaveLoginData()
    {
        JsonMgr.Instance.SaveData(loginData, "LoginData");
    }

    /// <summary>
    /// 注册成功后 清除登录数据
    /// </summary>
    public void ClearLoginData()
    {
        loginData.rememberMe = false;
        loginData.autoLogin = false;
        loginData.frontServerId = -1;
    }
    /// <summary>
    /// 保存注册数据
    /// </summary>
    public void SaveRegisterData()
    {
        JsonMgr.Instance.SaveData(registerData, "RegisterData");
    }

    /// <summary>
    /// 验证注册用户信息
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public bool RegisterUser(string username, string password)
    {
        //尝试在字典中找该用户名
        if(RegisterData.registerInfo.TryGetValue(username, out string pw))
        {
            //找到了，说明用户名已存在
            UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名已存在，请重新输入！");
            return false;
        }
        //没找到，说明用户名可用，进行注册
        RegisterData.registerInfo.Add(username, password);
        SaveRegisterData();
        return true;
    }

    /// <summary>
    /// 验证登录用户名密码是否正确
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public bool CheckLoginInfo(string username, string password)
    {
        //尝试在字典中找该用户名 TryGetValue会out出username对应的密码pw
        if(RegisterData.registerInfo.TryGetValue(username, out string pw))
        {
            // pw是否正确
            if(pw == password)
            {
                //密码正确，登录成功
                return true;
            }
        }
        //用户名不存在或密码错误
        UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码错误，请重新输入！");
        return false;
    }

}

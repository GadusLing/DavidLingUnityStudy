using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    /// <summary>
    /// 注册按钮
    /// </summary>
    public Button btnRegi;
    /// <summary>
    /// 登录按钮
    /// </summary>
    public Button btnConfirm;
    /// <summary>
    /// 用户名输入框
    /// </summary>
    public TMP_InputField inputUN;
    /// <summary>
    /// 密码输入框
    /// </summary>
    public TMP_InputField inputPW;
    /// <summary>
    /// 记住密码复选框
    /// </summary>
    public Toggle toggleRM;
    /// <summary>
    /// 自动登录复选框
    /// </summary>
    public Toggle toggleAL;
    public override void Init()
    {
        // 点击注册干什么
        btnRegi.onClick.AddListener(() =>
        {
            // 显示注册面板
            UIManager.Instance.ShowPanel<RegisterPanel>();
            // 隐藏当前登录面板
            UIManager.Instance.HidePanel<LoginPanel>();
            
        });
        // 点击登录干什么
        btnConfirm.onClick.AddListener(() =>
        {
            // 验证用户名和密码
            // 用户名不能小于6位，密码不能小于8位
            if(inputUN.text.Length < 6)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名不能小于6位！");
                return;
            }
            if(inputPW.text.Length < 8)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("密码不能小于8位！");
                return;
            }
            // 密码检查
            if(LoginMgr.Instance.CheckLoginInfo(inputUN.text, inputPW.text))
            {
                // 登录成功
                // 保存登录数据
                LoginMgr.Instance.LoginData.username = inputUN.text;
                LoginMgr.Instance.LoginData.password = inputPW.text;
                LoginMgr.Instance.LoginData.rememberMe = toggleRM.isOn;
                LoginMgr.Instance.LoginData.autoLogin = toggleAL.isOn;
                LoginMgr.Instance.SaveLoginData();
                // 显示选服界面
                if(LoginMgr.Instance.LoginData.frontServerId <= 0)
                {
                    // 如果从来没选择过服务器，直接打开选服面板
                    UIManager.Instance.ShowPanel<ChooseServerPanel>();
                }
                else
                {
                    // 之前有玩过，打开服务器面板(正式进入的最后一步)
                    UIManager.Instance.ShowPanel<ServerPanel>();
                }
                
                //隐藏当前登录面板
                UIManager.Instance.HidePanel<LoginPanel>();
            }
            else
            {
                // 登录失败
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码错误！");
                return;
            }          
        });

        // 勾选记住密码干什么
        toggleRM.onValueChanged.AddListener((isOn) =>
        {
            // 如果取消记住密码，则自动取消自动登录
            if (!isOn)
                toggleAL.isOn = false;
            
        });

        // 勾选自动登录干什么
        toggleAL.onValueChanged.AddListener((isOn) =>
        {
            // 选中自动登录时，必须选中记住密码
            if (isOn)
                toggleRM.isOn = true;
            
        });
        
    }

    public override void ShowMe()
    {
         base.ShowMe();
        // 显示时，加载登录数据到界面
        LoginData data = LoginMgr.Instance.LoginData;

        if (data != null)
        {
            inputUN.text = data.username;
            inputPW.text = data.rememberMe ? data.password : "";
            toggleRM.isOn = data.rememberMe;
            toggleAL.isOn = data.autoLogin;
        }

        // 如果是自动登录，则直接点击登录按钮
        if (data != null && toggleAL.isOn)
        {
            // 验证用户名和密码
            if(LoginMgr.Instance.CheckLoginInfo(inputUN.text, inputPW.text))
            {
                // 显示选服界面
                if(LoginMgr.Instance.LoginData.frontServerId <= 0)
                {
                    // 如果从来没选择过服务器，直接打开选服面板
                    UIManager.Instance.ShowPanel<ChooseServerPanel>();
                }
                else
                {
                    // 之前有玩过，打开服务器面板(正式进入的最后一步)
                    UIManager.Instance.ShowPanel<ServerPanel>();
                }
                
                //隐藏当前登录面板
                UIManager.Instance.HidePanel<LoginPanel>(false);
            }
            else
            {
                // 登录失败
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码错误！");
                return;
            }
        }
    }

    /// <summary>
    /// 提供给外部快捷设置用户名和密码的方法
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public void SetInfo(string username, string password)
    {
        inputUN.text = username;
        inputPW.text = password;
    }

}

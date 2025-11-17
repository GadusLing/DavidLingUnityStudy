using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerPanel : BasePanel
{
    /// <summary>
    /// 切换服务器按钮
    /// </summary>
    public Button btnChange;
    /// <summary>
    /// 开始游戏按钮
    /// </summary>
    public Button btnStart;
    /// <summary>
    /// 返回按钮
    /// </summary>
    public Button btnBack;
    /// <summary>
    /// 服务器名称文本
    /// </summary>
    public TMP_Text txtName;
    public override void Init()
    {
        btnBack.onClick.AddListener(() =>
        {
            if(LoginMgr.Instance.LoginData.autoLogin)
                LoginMgr.Instance.LoginData.autoLogin = false;
            // 返回登录面板
            UIManager.Instance.ShowPanel<LoginPanel>();
            // 隐藏当前面板
            UIManager.Instance.HidePanel<ServerPanel>();
        });
        btnChange.onClick.AddListener(() =>
        {
            // 换区 显示服务器列表面板
            UIManager.Instance.ShowPanel<ChooseServerPanel>();
            // 隐藏当前面板
            UIManager.Instance.HidePanel<ServerPanel>();
            
        });
        btnStart.onClick.AddListener(() =>
        {
            // 进入游戏主场景 由于过场景前面让Canvas不会被销毁 所以需要隐藏当前面板
            UIManager.Instance.HidePanel<ServerPanel>();
            UIManager.Instance.HidePanel<LoginBKPanel>();
            // 存储 当前选择的服务器ID
            LoginMgr.Instance.SaveLoginData();

            // 显示加载面板并开始异步加载游戏主场景
            UIManager.Instance.ShowPanel<LoadingPanel>().StartLoading("GameScene"); // 替换为您的实际场景名称
        });
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 显示时，加载上次登录的服务器到界面
        int id = LoginMgr.Instance.LoginData.frontServerId;
        if (id <= 0)
        {
            txtName.text = "无";
        }
        else
        {
            ServerInfo info = LoginMgr.Instance.ServerData[id - 1];
            txtName.text = info.id + "区 " + info.name;
        }
        
    }

}

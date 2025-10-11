using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerPanel : BasePanel<ServerPanel>
{
    private UILabel labInfo;
    private UIButton btnChange;
    private UIButton btnBegin;
    private UIButton btnBack;
    private Server tempSelectedServer; // 临时选择的服务器
    public override void Init()
    {
        labInfo = transform.Find("labInfo").GetComponent<UILabel>();
        btnChange = transform.Find("btnChange").GetComponent<UIButton>();
        btnBegin = transform.Find("btnBegin").GetComponent<UIButton>();
        btnBack = transform.Find("btnBack").GetComponent<UIButton>();

        btnChange.onClick.Add(new EventDelegate(() =>
        {
            ChooseServerPanel.Instance.ShowMe();
            HideMe();
        }));

        btnBegin.onClick.Add(new EventDelegate(() =>
        {
            //登录游戏成功，将选择的服务器ID作为“上次登录的服务器ID”保存
            if (tempSelectedServer != null)
            {
                LoginMgr.Instance.LoginData.frontServerID = tempSelectedServer.id;
            }
            LoginMgr.Instance.SaveLoginData(); // 保存所有登录数据，下次再登录就能加载上去
            SceneManager.LoadScene("GameScene");
        }));

        btnBack.onClick.Add(new EventDelegate(() =>
        {
            //返回登录面板
            LoginPanel.Instance.ShowMe();
            HideMe();
        }));

        HideMe();
    }

    //设置临时服务器的方法
    public void SetTempServer(Server server)
    {
        tempSelectedServer = server;
    }

    public override void ShowMe()
    {
        base.ShowMe();

        //显示时更新显示信息
        Server info;
        if (tempSelectedServer != null)
        {
            info = tempSelectedServer;
        }
        else
        {
            info = LoginMgr.Instance.ServerInfo.servers[LoginMgr.Instance.LoginData.frontServerID];
            tempSelectedServer = info; // 初始化临时服务器为上次登录的
        }
        
        labInfo.text = info.id + " 区 " + info.serverName;
    }

}

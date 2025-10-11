using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerChooseItem : MonoBehaviour
{
    private UIButton btnChooseServer; // 选择服务器按钮
    private UISprite sprNew;      // 新服标记
    private UISprite sprState;    // 服务器状态标记
    private UILabel labName;      // 服务器名称标签
    private Server nowInfo;   // 当前服务器信息 记录下来之后可以传给服务器面板去使用

    void Awake()
    {
        // 按照Hierarchy中看到的层级结构
        btnChooseServer = GetComponent<UIButton>();  // 按钮组件在当前物体上
        sprNew = transform.Find("sprNew").GetComponent<UISprite>();
        sprState = transform.Find("sprState").GetComponent<UISprite>();
        labName = transform.Find("labName").GetComponent<UILabel>();
    }
    
    void Start()
    {
        // 添加按钮点击事件
        btnChooseServer.onClick.Add(new EventDelegate(() =>
        {
            // 点击后更新服务器面板的信息
            ServerPanel.Instance.SetTempServer(nowInfo);// 记录当前选择的服务器ID作为临时ID传给服务器面板
            ServerPanel.Instance.ShowMe();
            ChooseServerPanel.Instance.HideMe();
        }));
    }

    // 设置服务器面板信息的方法
    public void InitInfo(Server info)
    {
        // 记录当前服务器信息
        nowInfo = info;
        // 根据服务器信息更新UI显示
        labName.text = info.id + " 区 " + info.serverName;
        sprNew.gameObject.SetActive(info.isNew);
        sprState.gameObject.SetActive(true);
        switch (info.state)
        {
            case 0:     // 不显示
                sprState.gameObject.SetActive(false);
                break;
            case 1:
                sprState.spriteName = "ui_DL_liuchang_01";  // 流畅
                break;
            case 2:
                sprState.spriteName = "ui_DL_fanhua_01";    // 繁忙
                break;
            case 3:
                sprState.spriteName = "ui_DL_huobao_01";    // 火爆
                break;
            case 4:
                sprState.spriteName = "ui_DL_weihu_01";     // 维护
                break;
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Login;

public class ChooseServerPanel : BasePanel
{
    public ScrollRect svLeft;
    public ScrollRect svRight;
    /// <summary>
    /// 上次登录服务器显示文本
    /// </summary>
    public TMP_Text txtFront;
    public Image imgState;
    /// <summary>
    /// 当前选择服务器范围显示文本
    /// </summary>
    public TMP_Text txtRange;
    /// <summary>
    /// 存储右侧服务器按钮的列表，负责右侧服务器按钮的动态生成与销毁
    /// </summary>
    public List<GameObject> itemList = new List<GameObject>();

    public override void Init()
    {
        // 动态创建左侧服务器按钮列表

        // 获取服务器列表的数据
        List<ServerInfo> infoList = LoginMgr.Instance.ServerData;
        int num = infoList.Count / 5 + 1;
        for (int i = 0; i < num; i++)
        {
            // 创建左侧按钮
            GameObject item = Instantiate(Resources.Load<GameObject>("UIPanelPrefab/ServerLeftItem"), svLeft.content);
            // 初始化左侧按钮显示内容
            int beginIndex = i * 5;
            int endIndex = Mathf.Min(beginIndex + 4, infoList.Count - 1);
            item.GetComponent<ServerLeftItem>().InitInfo(beginIndex, endIndex);

        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 显示自己时
        // 应该初始化上次登录服务器显示
        int id = LoginMgr.Instance.LoginData.frontServerId;
        if (id <= 0)
        {
            txtFront.text = "无";
            imgState.gameObject.SetActive(false);
        }
        else
        {
            ServerInfo info = LoginMgr.Instance.ServerData[id - 1];
            txtFront.text = info.id + "区  " + info.name;
            imgState.gameObject.SetActive(true);
            // 加载图集
            SpriteAtlas sa = Resources.Load<SpriteAtlas>("Login");
            switch (info.state)//0：默认不显示  1：流畅  2：繁忙  3：火爆  4：维护
            {
                case 0:
                    imgState.gameObject.SetActive(false);
                    break;
                case 1:
                    imgState.sprite = sa.GetSprite("ui_DL_liuchang_01");
                    break;
                case 2:
                    imgState.sprite = sa.GetSprite("ui_DL_fanhua_01");
                    break;
                case 3:
                    imgState.sprite = sa.GetSprite("ui_DL_huobao_01");
                    break;
                case 4:
                    imgState.sprite = sa.GetSprite("ui_DL_weihu_01");
                    break;
            }
        }
        //更新当前选择服务器范围的显示
        UpdatePanel(0, 4 > LoginMgr.Instance.ServerData.Count - 1 ? LoginMgr.Instance.ServerData.Count - 1 : 4);
    }

    /// <summary>
    /// 提供给外部，用于更新当前选择服务器范围对应的右侧服务器显示 的方法
    /// </summary>
    /// <param name="beginIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    public void UpdatePanel(int beginIndex, int endIndex)
    {
        // 更新当前选择服务器范围的显示
        txtRange.text = "服务器" + (beginIndex + 1) + " - " + (endIndex + 1);

        // 删除之前的右侧按钮
        foreach (GameObject go in itemList)
        {
            Destroy(go);
        }
        itemList.Clear();

        // 创建新的右侧按钮
        for (int i = beginIndex; i <= endIndex; i++)
        {
            // 创建右侧按钮
            ServerInfo nowInfo = LoginMgr.Instance.ServerData[i];
            GameObject serveritem = Instantiate(Resources.Load<GameObject>("UIPanelPrefab/ServerRightItem"), svRight.content);
            // 初始化右侧按钮显示内容
            serveritem.GetComponent<ServerRightItem>().InitInfo(nowInfo);
            // 添加到列表中以便后续删除
            itemList.Add(serveritem);
        }
    }
}

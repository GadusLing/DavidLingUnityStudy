using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseServerPanel : BasePanel<ChooseServerPanel>
{
    // 两个滚动视图，因为不需要修改滚动参数，只需要作为父对象来在其下方添加子对象，所以用transform类型即可
    private Transform svLeft; // 左侧区间列表
    private Transform svRight; // 右侧具体服务器列表

    private UILabel labName; // 顶部显示当前选择的服务器名称
    private UISprite sprState; // 顶部显示当前选择的服务器状态  
    private UILabel labNowServer; // 选服面板上方显示当前服务器区间
    private List<GameObject> serverChooseItems = new List<GameObject>(); // 存储右侧服务器列表的所有项

    public override void Init()
    {
        svLeft = transform.Find("svLeft").GetComponent<Transform>();
        svRight = transform.Find("svRight").GetComponent<Transform>();
        labName = transform.Find("labName").GetComponent<UILabel>();
        sprState = transform.Find("sprState").GetComponent<UISprite>();
        labNowServer = transform.Find("labNowServer").GetComponent<UILabel>();

        ServerInfo info = LoginMgr.Instance.ServerInfo;
        int serverCount = info.servers.Count / 5 + 1; // 每个区间5个服务器 例：23个服务器 23/5 + 1 = 5个区间

        for (int i = 0; i < serverCount; i++)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>("UI/btnServer"), svLeft);// Instantiate(prefab, parent) = Instantiate(prefab); item.transform.SetParent(parent, false);
            item.transform.localPosition = new Vector3(-85, 58, 0) + new Vector3(0, i * -65, 0); // 设置位置
            item.GetComponent<ServerItem>().InitInfo(i * 5 + 1, Mathf.Min((i + 1) * 5, info.servers.Count));
        }
        HideMe();
    }

    // 提供方法给外部根据左边的选区，更新右部具体服务器的信息
    public void UpdatePanel(int beginIndex, int endIndex)
    {
        // 更新右侧服务器发光的那个标识区 服务器XX-XX
        labNowServer.text = "服务器" + beginIndex + " - " + endIndex + "区";

        // 根据左边的选区范围创建右边的具体服务器按钮 每次创建新一批时要把老一批清除掉
        for (int i = serverChooseItems.Count - 1; i >= 0; i--)
        {
            Destroy(serverChooseItems[i].gameObject);
        }
        serverChooseItems.Clear();// 清空列表
        
        // 创建新按钮
        Server nowInfo;
        for (int i = beginIndex; i <= endIndex; i++)
        {
            nowInfo = LoginMgr.Instance.ServerInfo.servers[i];// 获取当前服务器信息

            GameObject item = Instantiate(Resources.Load<GameObject>("UI/btnChooseServer"), svRight);
            item.transform.localPosition = new Vector3(7, 56, 0) + new Vector3((i - 1) % 5 % 2 * 300, (i - 1) % 5 / 2 * -80, 0); // 设置位置

            item.GetComponent<ServerChooseItem>().InitInfo(nowInfo);// 初始化单个按钮上的信息

            serverChooseItems.Add(item);// 添加到列表中，方便后续清除
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        
        if(LoginMgr.Instance.LoginData.frontServerID != 0)
        {
            // 如果上次登录的服务器ID不为空，则直接显示该服务器信息
            Server server = LoginMgr.Instance.ServerInfo.servers[LoginMgr.Instance.LoginData.frontServerID];
            labName.text = server.id + " 区 " + server.serverName;
            sprState.gameObject.SetActive(true);
            switch (server.state)
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
        else
        {
            labName.text = "请选择服务器";
            sprState.gameObject.SetActive(false);
        }
        UpdatePanel(1, Mathf.Min(5, LoginMgr.Instance.ServerInfo.servers.Count)); // 默认显示第一个区间的服务器
    }
}

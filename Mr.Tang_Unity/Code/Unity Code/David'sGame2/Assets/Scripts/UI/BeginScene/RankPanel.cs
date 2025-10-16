using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : BasePanel<RankPanel>
{
    private UIButton btnClose;
    private UIScrollView svList;
    private List<RankItem> itemList = new List<RankItem>();// 用于存放单条排行数据控件的列表

    public override void Init()
    {
        btnClose = transform.Find("btnClose").GetComponent<UIButton>();
        svList = transform.Find("svList").GetComponent<UIScrollView>();

        btnClose.onClick.Add(new EventDelegate(() =>
        {
            HideMe();
        }));
        HideMe();
        // 测试排行榜数据
        // for (int i = 0; i < 7; i++)
        // {
        //     GameDataMgr.Instance.SaveRankData("玩家" + (i + 1), Random.Range(100, 5000));
        // }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 显示自己时根据上次保存的数据更新面板状态
        List<RankInfo> list = GameDataMgr.Instance.rankData.rankList;// 获取本地Xml文件中保存的排行数据
        for (int i = 0; i < list.Count; i++)
        {
            if (itemList.Count > i)// 如果itemList里已经存在足够的排行数据控件 直接更新数据即可 在GDMgr里已经排序过了
            {
                itemList[i].InitInfo(i + 1, list[i].name, list[i].time);
            }
            else// 如果控件不够 则需要实例化新的控件
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("UIPrefabs/RankItem"), svList.transform);
                
                // 手动设置 Anchor
                UIWidget widget = obj.GetComponent<UIWidget>();
                if (widget != null)
                {
                    int originalHeight = widget.height; // 记录预制体原始高度

                    // 每个 Item 根据索引 i 向下偏移 60 像素(控件的高度+间隔)
                    int bottomOffset = -57 - i * 60;  // 第一个 -57，第二个 -117，第三个 -177...
                    int topOffset = -4 - i * 60;      // 第一个 -4，第二个 -64，第三个 -124...
                    
                    widget.SetAnchor(svList.gameObject, 3, bottomOffset, 642, topOffset);
                    widget.leftAnchor.relative = 0f;    // Target's Left
                    widget.rightAnchor.relative = 0f;   // Target's Left
                    widget.bottomAnchor.relative = 1f;  // Target's Top
                    widget.topAnchor.relative = 1f;     // Target's Top
                    widget.topAnchor.target = null; // 解绑顶部和底部锚点，让排行榜可以滚动
                    widget.bottomAnchor.target = null; // 解绑底部锚点，避免滚动时被压扁

                    widget.UpdateAnchors();
                    widget.height = originalHeight; // 由于解绑了top和bottom锚点，item的高会被拉伸，所以需要恢复原始高度
                }
                
                RankItem item = obj.GetComponent<RankItem>();
                item.InitInfo(i + 1, list[i].name, list[i].time);
                itemList.Add(item);
            }
        }
        
        // 刷新 ScrollView 的边界
        svList.ResetPosition();
        svList.UpdateScrollbars();
    }
}

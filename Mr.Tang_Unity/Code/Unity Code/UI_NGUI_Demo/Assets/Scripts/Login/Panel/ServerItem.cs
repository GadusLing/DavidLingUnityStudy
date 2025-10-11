using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 自定义服务器列表项
/// </summary>
public class ServerItem : MonoBehaviour
{
    private UIButton btnServer;
    private UILabel labInfo;
    private int beginIndex;
    private int endIndex;

    void Awake()
    {
        // 注意这里路径应该要跟Hierarchy面板中的层级结构一致
        btnServer = transform.GetComponent<UIButton>();  // 按钮组件在当前物体上
        labInfo = transform.Find("labInfo").GetComponent<UILabel>();
    }

    void Start()
    {
        btnServer.onClick.Add(new EventDelegate(() =>
        {
            ChooseServerPanel.Instance.UpdatePanel(beginIndex, endIndex);
        }));
    }

    // 面板上的ID区间 例：1 - 10区
    public void InitInfo(int beginIndex, int endIndex)
    {
        // 记录按钮区间，方便后面动态创建单个服务器面板
        this.beginIndex = beginIndex;
        this.endIndex = endIndex;

        labInfo.text = beginIndex + " - " + endIndex + "区";
    }

}

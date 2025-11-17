using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerLeftItem : MonoBehaviour
{
    /// <summary>
    /// 按钮自身
    /// </summary>
    public Button btnSelf;
    /// <summary>
    /// 服务器区间信息
    /// </summary>
    public TMP_Text txtInfo;
    /// <summary>
    /// 区间开始索引
    /// </summary>
    private int beginIndex;
    /// <summary>
    /// 区间结束索引
    /// </summary>
    private int endIndex;
    void Start()
    {
        btnSelf.onClick.AddListener(() =>
        {
            // 通知选服面板改变右侧的服务器区间显示
            ChooseServerPanel panel = UIManager.Instance.GetPanel<ChooseServerPanel>();
            panel.UpdatePanel(beginIndex, endIndex);
            
        });
    }

    public void InitInfo(int begin, int end)
    {
        beginIndex = begin;
        endIndex = end;
        txtInfo.text = $"{beginIndex + 1}  -  {endIndex + 1}区";
    }

}

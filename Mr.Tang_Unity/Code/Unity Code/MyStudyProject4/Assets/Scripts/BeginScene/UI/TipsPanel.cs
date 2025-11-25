using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    public Button btnSure;
    public TMP_Text txtInfo;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipsPanel>();
        });
    }

    /// <summary>
    /// 修改提示信息
    /// </summary>
    /// <param name="info">传入要显示的提示信息</param>
    public void changeInfo(string info)
    {
        txtInfo.text = info;
    }
}

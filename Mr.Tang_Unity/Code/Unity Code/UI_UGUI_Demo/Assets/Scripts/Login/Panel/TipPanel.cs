using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Button btnConfirm;
    public TMP_Text txtInfo;
    public override void Init()
    {
        btnConfirm.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }

    /// <summary>
    /// 修改提示信息
    /// </summary>
    /// <param name="info">提示信息</param>
    public void ChangeInfo(string info)
    {
        txtInfo.text = info;
    }


}

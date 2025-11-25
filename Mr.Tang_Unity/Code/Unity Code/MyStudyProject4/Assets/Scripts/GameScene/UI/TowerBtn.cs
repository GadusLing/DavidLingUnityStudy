using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    public Image imgPic;
    public TMP_Text txtTip;
    public TMP_Text txtGold;

    /// <summary>
    /// 提供给外部初始化塔按钮显示信息的方法
    /// </summary>
    /// <param name="id">塔的ID</param>
    /// <param name="inputStr">要显示的提示文本</param>
    public void InitInfo(int id, string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        if(info != null)
        {
            imgPic.sprite = Resources.Load<Sprite>(info.imgRes);
            txtTip.text = inputStr;
            txtGold.text = $"${info.cost}";

            if(GameLevelMgr.Instance.player.Gold < info.cost)
            {
                txtGold.text = "金币不足";
            }
        }
    }

}

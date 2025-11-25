using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameOverPanel : BasePanel
{
    public TMP_Text txtWin;
    public TMP_Text txtInfo;
    public TMP_Text txtGold;
    public Button btnOK;

    public override void Init()
    {
        btnOK.onClick.AddListener(() =>
        {
            // 隐藏游戏结束面板和游戏面板 显示主菜单面板
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();
            GameLevelMgr.Instance.ClearLevelInfo();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int gold, bool isWin)
    {
  
        txtWin.text = isWin ? "通关!" : "失败!";
        txtInfo.text = isWin ? "恭喜你成功通关!" : "很遗憾未能保护好圣像!";
        txtGold.text = $"获得$ {gold}";

        // 根据奖励金币数更新玩家总金币数
        GameDataMgr.Instance.playerData.gold += gold;
        GameDataMgr.Instance.SaveGameData<PlayerData>();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 解锁鼠标
        Cursor.lockState = CursorLockMode.None;
    }
}
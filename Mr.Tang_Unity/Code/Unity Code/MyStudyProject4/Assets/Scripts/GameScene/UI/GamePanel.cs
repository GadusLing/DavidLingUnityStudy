using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    /// <summary>
    /// 血条图片
    /// </summary>
    public Image imgHP;
    /// <summary>
    /// 血量文本
    /// </summary>
    public TMP_Text txtHP;
    /// <summary>
    /// 怪物波数文本
    /// </summary>
    public TMP_Text txtWave;
    /// <summary>
    /// 金币文本
    /// </summary>
    public TMP_Text txtGold;
    /// <summary>
    /// hp 的初始宽，可以在外部控制它的宽
    /// </summary>
    public float hpW = 500;
    /// <summary>
    /// 退出按钮
    /// </summary>
    public Button btnQuit;
    /// <summary>
    /// 下方造塔组合控件的父对象 主要用于控制 显隐
    /// </summary>
    public Transform botTrans;
    /// <summary>
    /// 管理三个造塔按钮的列表
    /// </summary>
    public List<TowerBtn> towerBtnList = new List<TowerBtn>();
    /// <summary>
    /// 当前进入和选中的造塔点
    /// </summary>
    private TowerPoint curSelTowerPoint;
    /// <summary>
    /// 用来标识 是否检测造塔输入
    /// </summary>
    private bool checkInput = false;

    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            // 隐藏游戏面板 显示主菜单面板
            UIManager.Instance.HidePanel<GamePanel>();
            // 解锁鼠标
            Cursor.lockState = CursorLockMode.None;
            // 清除当前关卡信息 返回主菜单场景
            GameLevelMgr.Instance.ClearLevelInfo();
            SceneManager.LoadScene("BeginScene");
        });
        // 一开始隐藏下方造塔组合控件 到了造塔点再显示
        botTrans.gameObject.SetActive(false);

        // 锁定鼠标在游戏窗口内
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 更新圣像(主基地)血量
    /// </summary>
    /// <param name="curHP">当前血量</param>
    /// <param name="maxHP">最大血量</param>
    public void UpdateMainHP(int curHP, int maxHP)
    {
        // 更新血条长度
        imgHP.rectTransform.sizeDelta = new Vector2(hpW * curHP / maxHP, imgHP.rectTransform.sizeDelta.y);
        // 更新血量文本
        txtHP.text = $"{curHP}/{maxHP}";
    }
    /// <summary>
    /// 更新玩家金币显示
    /// </summary>
    /// <param name="wave">当前波数</param>
    /// <param name="maxWave">最大波数</param>
    public void UpdateWave(int wave, int maxWave)
    {
        txtWave.text = $"{wave} / {maxWave}";
    }

    public void UpdateGold(int gold)
    {
        txtGold.text = $"{gold}";
    }

    public void UpdateSelTower(TowerPoint point)
    {
        curSelTowerPoint = point;
        // 如果传入的造塔点是null 就隐藏造塔按钮面板 并返回
        if(curSelTowerPoint == null)
        {
            checkInput = false;
            // 隐藏造塔按钮面板
            botTrans.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            // 显示造塔按钮父对象
            botTrans.gameObject.SetActive(true);
            if(curSelTowerPoint.curTowerInfo == null)
            {
                // 更新三个造塔按钮显示内容
                for(int i = 0; i < towerBtnList.Count; i++)
                {
                    // 显示造塔控件
                    towerBtnList[i].gameObject.SetActive(true);
                    towerBtnList[i].InitInfo(curSelTowerPoint.chooseIDList[i], $"数字键{i + 1}");
                }
            }
            else
            {
                // 有造塔信息证明之前有塔 进入升级流程 只需要中间的格子 隐藏两边的造塔控件
                for (int i = 0; i < towerBtnList.Count; i++)
                {
                    towerBtnList[i].gameObject.SetActive(false);
                }
                towerBtnList[1].gameObject.SetActive(true);
                towerBtnList[1].InitInfo(curSelTowerPoint.curTowerInfo.nextLevelId, "按T键升级");
            }
        }
    }

    /// <summary>
    /// 主要用于检测玩家是否按下了快捷键来切换造塔按钮的状态
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if(checkInput && curSelTowerPoint != null)
        {
            // 如果当前没有塔 则检测数字键1 2 3
            if(curSelTowerPoint.curTowerInfo == null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    curSelTowerPoint.CreatTower(curSelTowerPoint.chooseIDList[0]);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    curSelTowerPoint.CreatTower(curSelTowerPoint.chooseIDList[1]);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    curSelTowerPoint.CreatTower(curSelTowerPoint.chooseIDList[2]);
                }
            }
            else // 有塔则检测空格键
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    curSelTowerPoint.CreatTower(curSelTowerPoint.curTowerInfo.nextLevelId);
                }
            }
        }
    }
}

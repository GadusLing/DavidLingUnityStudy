using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class ChooseHeroPanel : BasePanel
{
    /// <summary>
    /// 切换人物 左
    /// </summary>
    public Button btnLeft;
    /// <summary>
    /// 切换人物 右
    /// </summary>
    public Button btnRight;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public Button btnStart;
    /// <summary>
    /// 返回
    /// </summary>
    public Button btnBack;
    /// <summary>
    /// 解锁人物
    /// </summary>
    public Button btnUnlock;
    /// <summary>
    /// 左上角 现有金币
    /// </summary>
    public TMP_Text txtMoney;
    /// <summary>
    /// 解锁人物所需金币
    /// </summary>
    public TMP_Text txtUnlock;
    /// <summary> 
    /// 人物名称
    /// </summary>
    public TMP_Text txtName;
    /// <summary>
    /// 英雄位置指示点父物体
    /// </summary>
    private Transform heroPos;
    /// <summary>
    /// 当前显示的英雄物体
    /// </summary>
    private GameObject heroObj;
    /// <summary>
    /// 当前选择的英雄信息
    /// </summary>
    private RoleInfo curRoleInfo;
    /// <summary>
    /// 当前角色列表索引
    /// </summary>
    private int curRoleListIndex = 0;
    public override void Init()
    {
        // 找到 场景中的英雄位置指示点父物体
        heroPos = GameObject.Find("HeroPos").transform;

        // 更新左上角金币显示
        txtMoney.text = GameDataMgr.Instance.playerData.gold.ToString();

        // 左切换按钮
        btnLeft.onClick.AddListener(() =>
        {
            --curRoleListIndex;
            if (curRoleListIndex < 0)
                curRoleListIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            ChangeHeroDisplay();
        });

        // 右切换按钮
        btnRight.onClick.AddListener(() =>
        {
            ++curRoleListIndex;
            if (curRoleListIndex >= GameDataMgr.Instance.roleInfoList.Count)
                curRoleListIndex = 0;
            ChangeHeroDisplay();
        });

        // 开始游戏按钮
        btnStart.onClick.AddListener(() =>
        {
            // 记录当前选择的角色
            GameDataMgr.Instance.curChooseRole = curRoleInfo;
            // 切换到选择游戏场景面板
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
            // 隐藏当前面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });

        // 解锁按钮
        btnUnlock.onClick.AddListener(() =>
        {
            // 解锁角色
            PlayerData pd = GameDataMgr.Instance.playerData;
            if (pd.gold >= curRoleInfo.lockMoney)
            {
                pd.gold -= curRoleInfo.lockMoney;
                pd.ownedRoleIds.Add(curRoleInfo.id);
                // 保存玩家数据
                GameDataMgr.Instance.SaveGameData<PlayerData>();
                // 更新金币显示
                txtMoney.text = pd.gold.ToString();
                // 更新解锁按钮显示
                UpdateLockBtn();
                UIManager.Instance.ShowPanel<TipsPanel>().changeInfo("角色解锁成功！");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipsPanel>().changeInfo("金币不足，无法解锁该角色！");
            }
        });

        // 返回按钮
        btnBack.onClick.AddListener(() =>
        {
            // 播放摄像机右转动画
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                // 显示开始面板
                UIManager.Instance.ShowPanel<BeginPanel>();
            });

            // 隐藏当前面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });
        // 初始化显示第一个英雄
        ChangeHeroDisplay();
    }

    /// <summary>
    /// 切换英雄显示
    /// </summary>
    private void ChangeHeroDisplay()
    {
        // 销毁旧的英雄物体
        if (heroObj != null)
            Destroy(heroObj);
        // 实例化新的英雄物体
        curRoleInfo = GameDataMgr.Instance.roleInfoList[curRoleListIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(curRoleInfo.PrefabRes), heroPos.position, heroPos.rotation, heroPos);
        //由于我们现在在对象上挂载了Playerobject但是在开始场景不需要
        Destroy(heroObj.GetComponent<PlayerObject>());
        // 更新Tips显示
        txtName.text = curRoleInfo.tips;

        // 根据解锁数据决定是否显示解锁按钮和解锁所需金币
        UpdateLockBtn();

    }

    /// <summary>
    /// 更新解锁按钮显示
    /// </summary>
    private void UpdateLockBtn()
    {
        //如果该角色需要解锁 就应该显示解锁按钮 并且隐藏开始按钮
        if (curRoleInfo.lockMoney > 0 && !GameDataMgr.Instance.playerData.ownedRoleIds.Contains(curRoleInfo.id))
        {
            btnUnlock.gameObject.SetActive(true);
            btnStart.gameObject.SetActive(false);
            txtUnlock.text = "$" + curRoleInfo.lockMoney.ToString();
        }
        else
        {
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callback)
    {
        base.HideMe(callback);
        // 销毁旧的英雄物体
        if (heroObj != null)
            DestroyImmediate(heroObj);
        heroObj = null;
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    /// <summary>
    /// 切换场景 左
    /// </summary>
    public Button btnLeft;
    /// <summary>
    /// 切换场景 右
    /// </summary>
    public Button btnRight;
    /// <summary>
    /// 开始游戏
    /// </summary>
    public Button btnStart;
    /// <summary>
    /// 返回按钮
    /// </summary>
    public Button btnBack;
    /// <summary>
    /// 场景名称文本
    /// </summary>
    public TMP_Text txtInfo;
    /// <summary>
    /// 场景图片资源
    /// </summary>
    public Image imgScene;

    // 当前场景索引
    private int curSceneIndex = 0;
    // 当前场景信息
    private SceneInfo curSceneInfo;

    public override void Init()
    {
        // 点击左按钮
        btnLeft.onClick.AddListener(() =>
        {
            --curSceneIndex;
            if (curSceneIndex < 0)
                curSceneIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            changeSceneDisplay();
        });
        // 点击右按钮
        btnRight.onClick.AddListener(() =>
        {
            ++curSceneIndex;
            if (curSceneIndex >= GameDataMgr.Instance.sceneInfoList.Count)
                curSceneIndex = 0;
            changeSceneDisplay();
        });
        // 点击开始游戏按钮
        btnStart.onClick.AddListener(() =>
        {
            // 隐藏选择场景面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // 切换到游戏场景  这里不能用同步 因为同步加载 开始场景销毁后 游戏场景不一定加载结束了 会找不到游戏场景中的英雄出生点 所以要用异步
            AsyncOperation ao = SceneManager.LoadSceneAsync(curSceneInfo.sceneObjName);
            // 初始化游戏关卡信息
            ao.completed += (obj) =>
            {
                GameLevelMgr.Instance.InitInfo(curSceneInfo);
            };
        });
        // 点击返回按钮
        btnBack.onClick.AddListener(() =>
        {
            // 隐藏选择场景面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // 显示角色选择面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        // 初始化显示第一个场景
        changeSceneDisplay();
    }

    /// <summary>
    /// 修改场景显示
    /// </summary>
    public void changeSceneDisplay()
    {
        curSceneInfo = GameDataMgr.Instance.sceneInfoList[curSceneIndex];
        txtInfo.text = "名称:\n" + curSceneInfo.name + "\n描述:\n" + curSceneInfo.tips;
        imgScene.sprite = Resources.Load<Sprite>(curSceneInfo.imgRes);
    }
}

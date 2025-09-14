using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginPanel : MonoBehaviour
{
    private static BeginPanel instance;

    public static void Showme()
    {
        if(instance != null) instance.gameObject.SetActive(true);
    }
    public static void Hideme()
    {
        if(instance != null) instance.gameObject.SetActive(false);
    }

    public Rect BKRect;// 背景区域
    public Texture2D BKTexture;// 背景图片

    [Header("标题设置")]
    public Rect TitleRect;// 标题区域
    public GUIContent TitleContent;// 标题内容

    [Header("按钮设置")]
    public GUIStyle TitleStyle;// 标题样式
    public Rect button1Rect1;// 开始
    public Rect button1Rect2;// 设置
    public Rect button1Rect3;// 成就/排行榜
    public Rect button1Rect4;// 制作者
    public Rect button1Rect5;// 退出
    public GUIStyle buttonStyle;// 按钮样式

    private void Awake()
    {
        instance = this;
    }

    private void OnGUI()
    {
        //背景图片,绘制是有先后顺序的，背景要最先绘制
        GUI.DrawTexture(BKRect, BKTexture, ScaleMode.StretchToFill);

        //游戏标题
        GUI.Label(TitleRect, TitleContent, TitleStyle);

        if(GUI.Button(button1Rect1, "开始游戏", buttonStyle))
        {
            // 开始游戏
            LoginPanel.Showme();
            Hideme();
        }
        if(GUI.Button(button1Rect2, "设置", buttonStyle))
        {
            // 设置
            SettingPanel.Showme();
            Hideme();
        }
        if(GUI.Button(button1Rect3, "成就", buttonStyle))
        {
            // 成就
            AchievementPanel.Showme();
            Hideme();
        }
        if(GUI.Button(button1Rect4, "制作者们", buttonStyle))
        {
            // 制作者们
            CreditsPanel.Showme();
            Hideme();
        }
        if(GUI.Button(button1Rect5, "退出", buttonStyle))
        {
            // 退出
            QuitTipPanel.Showme();
        }
    }
}

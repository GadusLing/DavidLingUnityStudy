using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Scripting;

public class AchievementPanel : MonoBehaviour
{
    public Rect toolbarRect;
    public Rect selGridRect;
    public Rect labelRect;
    
    [Header("关闭按钮设置")]
    public Rect closeButtonRect;
    public GUIStyle buttonStyle;

    public string[] toolbarStrings = { "成就", "排行榜" };
    public int toolbarInt = 0;
    private static AchievementPanel instance;
    public static void Showme()
    {
        if(instance != null) instance.gameObject.SetActive(true);
    }
    public static void Hideme()
    {
        if(instance != null) instance.gameObject.SetActive(false);
    }
    public void Awake()
    {
        instance = this;
        Hideme();
    }

    public void OnGUI()
    {
        toolbarInt = GUI.Toolbar(toolbarRect, toolbarInt, toolbarStrings);
        toolbarInt = GUI.SelectionGrid(selGridRect, toolbarInt, toolbarStrings, 1);
        switch(toolbarInt)
        {
            case 0:
                GUI.Label(labelRect, "这里是成就界面");
                break;
            case 1:
                GUI.Label(labelRect, "这里是排行榜界面");
                break;
            default:
                break;
        }
        if(GUI.Button(closeButtonRect, "", buttonStyle))
        {
            Hideme();
            BeginPanel.Showme();
        }
    }
}

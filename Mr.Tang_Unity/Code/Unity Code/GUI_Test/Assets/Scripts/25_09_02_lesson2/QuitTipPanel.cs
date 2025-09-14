using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitTipPanel : MonoBehaviour
{
    private static QuitTipPanel instance;
    public static void Showme()
    {
        if(instance != null) instance.gameObject.SetActive(true);
    }
    public static void Hideme()
    {
        if(instance != null) instance.gameObject.SetActive(false);
    }

    [Header("窗口设置")]
    public Rect windowRect;// 窗口区域
    public Rect labelRect;// 提示语区域
    public GUIStyle labelStyle;// 提示语样式


    [Header("按钮设置")]
    public Rect yesButtonRect;// 是
    public Rect noButtonRect;// 否
    //public GUIStyle buttonStyle;// 按钮样式

    private void Awake()
    {
        instance = this;
        Hideme();
    }

    private void OnGUI()
    {
        GUI.ModalWindow(1, windowRect, DrawMyWindow, "退出提示");
    }
    private void DrawMyWindow(int windowID)
    {
        GUI.Label(labelRect, "这么好玩的游戏，主公真的要走吗？", labelStyle);
        if(GUI.Button(yesButtonRect, "是"))
        {
            Application.Quit();
        }
        if(GUI.Button(noButtonRect, "否"))
        {
            Hideme();
            BeginPanel.Showme();
        }
    }
}


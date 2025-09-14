using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    public Rect svRect;
    public Rect showRect;
    private Vector2 nowRect;

    public string[] strs;
    
    [Header("关闭按钮设置")]
    public Rect closeButtonRect;
    public GUIStyle buttonStyle;
    private static CreditsPanel instance;
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
        showRect.height = strs.Length * 30;
        nowRect = GUI.BeginScrollView(svRect, nowRect, showRect);
        for(int i = 0; i < strs.Length; i++)
        {
            GUI.Label(new Rect(0, i * 30, 100, 30), strs[i]);
        }
        GUI.EndScrollView();
        if(GUI.Button(closeButtonRect, "", buttonStyle))
        {
            Hideme();
            BeginPanel.Showme();
        }
    }
}

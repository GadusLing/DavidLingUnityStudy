using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    private static SettingPanel instance;
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
    public Rect toggleMusicRect;
    public Rect toggleSoundRect;
    public Rect closeButtonRect;

    [Header("开关设置")]
    public GUIStyle toggleStyle;

    [Header("关闭按钮设置")]
    public GUIStyle buttonStyle;

    private bool isMusicOn = true;
    private bool isSoundOn = true;
    private float Musicvolume = 0.5f;
    private float Soundvolume = 0.5f;
    public Rect musicVolumeRect;
    public Rect soundVolumeRect;

    private void Awake()
    {
        instance = this;
        Hideme();
    }

    private void OnGUI()
    {
        GUI.DrawTexture(BKRect, BKTexture, ScaleMode.StretchToFill);

        isMusicOn = GUI.Toggle(toggleMusicRect, isMusicOn, "音乐开关", toggleStyle);
        isSoundOn = GUI.Toggle(toggleSoundRect, isSoundOn, "音效开关", toggleStyle);
        Musicvolume = GUI.HorizontalSlider(musicVolumeRect, Musicvolume, 0, 1);
        Soundvolume = GUI.HorizontalSlider(soundVolumeRect, Soundvolume, 0, 1);

        if(GUI.Button(closeButtonRect, "", buttonStyle))
        {
            Hideme();
            BeginPanel.Showme();
        }
    }
}

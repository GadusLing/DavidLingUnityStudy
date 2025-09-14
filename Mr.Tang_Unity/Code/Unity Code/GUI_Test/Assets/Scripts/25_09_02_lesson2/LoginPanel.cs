using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{
    private static LoginPanel instance;
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

    [Header("登录面板设置")]
    public Rect userNameRect;
    public Rect userNameLabelRect;
    public Rect passwordRect;
    public Rect passwordLabelRect;
    public Rect loginButtonRect;
    public Rect returnButtonRect;
    public GUIStyle textFieldStyle;
    //public GUIStyle buttonStyle;

    private string userName = "";
    private string password = "";

    private void Awake()
    {
        instance = this;
        Hideme();
    }

    private void OnGUI()
    {
        GUI.DrawTexture(BKRect, BKTexture, ScaleMode.StretchToFill);

        GUI.Label(userNameLabelRect, "用户名:", textFieldStyle);
        GUI.Label(passwordLabelRect, "密  码:", textFieldStyle);
        userName = GUI.TextField(userNameRect, userName);
        password = GUI.PasswordField(passwordRect, password, '*');


        if(GUI.Button(loginButtonRect, "登录"))
        {
            // 登录
            if(userName == "admin" && password == "8888") 
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.Log("用户名或密码错误");
            }
        }
        if(GUI.Button(returnButtonRect, "返回"))
        {
            // 返回
            Hideme();
            BeginPanel.Showme();
        }
    }
}

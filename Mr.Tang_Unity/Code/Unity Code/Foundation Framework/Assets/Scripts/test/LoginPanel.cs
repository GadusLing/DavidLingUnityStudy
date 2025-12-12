using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        // GetTControl<Button>("btnStart").onClick.AddListener(() =>
        // {
        //     Debug.Log("点击了登录按钮");
        // });
        // GetTControl<Button>("btnQuit").onClick.AddListener(() =>
        // {
        //     Debug.Log("点击了退出按钮");
        // });

        // EventTrigger eventTrigger = GetTControl<Button>("btnStart").gameObject.AddComponent<EventTrigger>();
        // EventTrigger.Entry entry = new EventTrigger.Entry();
        // entry.eventID = EventTriggerType.Drag;
        // entry.callback.AddListener((data) =>
        // {

        // });
        // eventTrigger.triggers.Add(entry);

        UIManager.AddCustomEventListener(GetTControl<Button>("btnStart"), EventTriggerType.PointerEnter, (data) =>
        {
            Debug.Log("鼠标进入登录按钮");
        });

        UIManager.AddCustomEventListener(GetTControl<Button>("btnStart"), EventTriggerType.PointerExit, (data) =>
        {
            Debug.Log("鼠标离开登录按钮");
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestMethod()
    {
        Debug.Log("我是面板显示完毕之后的回调方法触发的哦");
    }

    override public void ShowMe()
    {
        Debug.Log("LoginPanel 显示出来了");
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnStart":
                Debug.Log("点击了登录按钮");
                break;
            case "btnQuit":
                Debug.Log("点击了退出按钮");
                break;
            default:
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, bool isOn)
    {
        switch (toggleName)
        {
            case "toggleRemember":
                Debug.Log($"记住密码切换为 {isOn}");
                break;
            case "toggleAuto":
                Debug.Log($"自动登录切换为 {isOn}");
                break;
            default:
                break;
        }
    }

}

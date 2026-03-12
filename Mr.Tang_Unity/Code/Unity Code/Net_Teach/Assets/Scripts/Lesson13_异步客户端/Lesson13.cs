using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson13 : MonoBehaviour
{
    public InputField input;
    public Button sendBtn;
    // Start is called before the first frame update
    void Start()
    {
        sendBtn.onClick.AddListener(() => // 给发送按钮添加点击事件监听器
        {
            if(input.text != "") // 如果输入框不为空
            {
                NetAsyncMgr.Instance.Send(input.text); // 通过NetAsyncMgr实例发送消息，消息内容来自输入框
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

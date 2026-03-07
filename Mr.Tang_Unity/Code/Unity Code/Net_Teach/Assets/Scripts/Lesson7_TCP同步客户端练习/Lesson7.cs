using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson7 : MonoBehaviour
{
    public Button btn;
    public InputField input;
    void Start()
    {
        btn.onClick.AddListener(() => // 给按钮添加点击事件监听器
        {
            if(input.text != null) // 如果输入框不为空
                NetManager.Instance.Send(input.text); // 通过NetManager发送消息到服务器
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

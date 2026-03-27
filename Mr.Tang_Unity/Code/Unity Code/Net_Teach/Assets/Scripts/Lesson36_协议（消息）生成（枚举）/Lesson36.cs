using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson36 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    #region 知识点一 协议(消息)生成主要做什么?
    // 协议生成，主要是使用配置文件中读取出来的信息
    // 动态的生成对应语言的代码文件
    // 每次添加消息或者数据结构类时，我们不需要再手写代码了
    // 我们不仅可以生成C#脚本文件，还可以根据需求生成别的语言的文件
    #endregion

    #region 知识点二 制作功能前的准备工作
    // 协议生成是不会在发布后使用的功能，主要是在开发时使用
    // 所以我们在Unity当中可以把它作为一个编辑器功能来做
    // 因此我们可以专门新建一个Editor文件夹（专门放编辑器相关内容，不会发布）
    // 在其中放置配置文件、自动生成相关脚本文件
    #endregion

    #region 知识点三 制作生成枚举功能
    // 1.读取xml枚举相关信息
    // 2.根据枚举相关信息 拼接字符串
    // 3.生成枚举脚本文件
    #endregion

    #region 总结
        
    #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

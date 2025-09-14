using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CustomGUIButton : CustomGUIControl
{
    /// <summary>
    /// 提供给外部用于响应按钮点击的事件 只要在外部注册该事件就会执行
    /// </summary>
    public event UnityAction clickEvent;

    protected override void StyleOnDraw()
    {
        if (GUI.Button(guiPos.GetRect(), guiContent, guiStyle))
        {
            clickEvent?.Invoke();
        }
    }

    protected override void StyleOffDraw()
    {
        if (GUI.Button(guiPos.GetRect(), guiContent))
        {
            clickEvent?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 样式开关枚举
/// </summary>
public enum E_Style_OnOff
{
    On,     // 使用自定义样式
    Off     // 使用默认样式
}

/// <summary>
/// 自定义GUI控件基类
/// </summary>
public abstract class CustomGUIControl : MonoBehaviour
{
    /// <summary>
    /// GUI位置信息
    /// </summary>
    public CustomGUIPos guiPos;
    
    /// <summary>
    /// GUI内容信息
    /// </summary>
    public GUIContent guiContent;
    
    /// <summary>
    /// GUI样式
    /// </summary>
    public GUIStyle guiStyle;
    
    /// <summary>
    /// 样式开关，控制是否使用自定义样式
    /// </summary>
    public E_Style_OnOff styleOnOff = E_Style_OnOff.Off;

    /// <summary>
    /// Unity的OnGUI方法，用于绘制GUI
    /// </summary>
    public void DrawGUI()
    {
        switch (styleOnOff)
        {
            case E_Style_OnOff.On:
                StyleOnDraw();
                break;
            case E_Style_OnOff.Off:
                StyleOffDraw();
                break;
        }
    }

    /// <summary>
    /// 使用自定义样式绘制控件
    /// </summary>
    protected abstract void StyleOnDraw();
    // 这里用抽象方法，实际的需求去子类里面重写
    
    /// <summary>
    /// 使用默认样式绘制控件
    /// </summary>
    protected abstract void StyleOffDraw();
}
    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义GUI位置计算类
/// 该类用于计算GUI元素在屏幕上的位置和大小
/// 不需要继承MonoBehaviour，因为它只是一个数据类
/// </summary>
[System.Serializable]// 没有这个标记，Inspector中不会显示 MonoBehaviour自带序列化 而 自定义类需要手动标记
public class CustomGUIPos
{
    /// <summary>
    /// 锚点枚举，定义GUI元素相对于屏幕的九宫格位置
    /// </summary>
    public enum E_Anchor
    {
        TopLeft,        // 左上角
        TopCenter,      // 上方中心
        TopRight,       // 右上角
        MiddleLeft,     // 左侧中心
        MiddleCenter,   // 屏幕中心
        MiddleRight,    // 右侧中心
        BottomLeft,     // 左下角
        BottomCenter,   // 下方中心
        BottomRight     // 右下角
    }

    /// <summary>
    /// 控件在屏幕上的对齐方式，默认锚点为中心
    /// </summary>
    public E_Anchor screenAnchor = E_Anchor.MiddleCenter;

    /// <summary>
    /// 控件自身的九宫格对齐方式，锚点默认为中心
    /// </summary>
    public E_Anchor objectAnchor = E_Anchor.MiddleCenter;

    /// <summary>
    /// GUI元素的大小的偏移位置，当大方向固定好之后可能还需要调整一些偏移,默认为零
    /// </summary>
    public Vector2 offset;

    /// <summary>
    /// GUI元素的大小（宽度和高度），默认为(100, 50)
    /// </summary>
    public Vector2 size = new Vector2(100, 50);

    /// <summary>
    /// 根据锚点、偏移量和大小计算GUI元素在屏幕上的矩形区域
    /// </summary>
    /// <returns>返回一个Rect对象，包含GUI元素的位置和大小信息</returns>
    public Rect GetRect()
    {
        // 第一步：根据屏幕九宫格锚点计算基准位置
        Vector2 screenPosition = GetAnchorPosition(screenAnchor, Screen.width, Screen.height);
        
        // 第二步：根据对象自身九宫格锚点计算对象的偏移量
        Vector2 objectOffset = GetAnchorPosition(objectAnchor, size.x, size.y);
        
        // 第三步：应用用户自定义的偏移量
        Vector2 finalPosition = screenPosition - objectOffset + offset;
        
        // 返回最终的矩形区域
        return new Rect(finalPosition.x, finalPosition.y, size.x, size.y);
    }
    
    /// <summary>
    /// 根据锚点和区域大小计算锚点位置
    /// </summary>
    /// <param name="anchor">锚点类型</param>
    /// <param name="width">区域宽度</param>
    /// <param name="height">区域高度</param>
    /// <returns>锚点在该区域内的位置坐标</returns>
    private Vector2 GetAnchorPosition(E_Anchor anchor, float width, float height)
    {
        switch (anchor)
        {
            case E_Anchor.TopLeft:
                return new Vector2(0, 0);
            case E_Anchor.TopCenter:
                return new Vector2(width * 0.5f, 0);
            case E_Anchor.TopRight:
                return new Vector2(width, 0);
            case E_Anchor.MiddleLeft:
                return new Vector2(0, height * 0.5f);
            case E_Anchor.MiddleCenter:
                return new Vector2(width * 0.5f, height * 0.5f);
            case E_Anchor.MiddleRight:
                return new Vector2(width, height * 0.5f);
            case E_Anchor.BottomLeft:
                return new Vector2(0, height);
            case E_Anchor.BottomCenter:
                return new Vector2(width * 0.5f, height);
            case E_Anchor.BottomRight:
                return new Vector2(width, height);
            default:
                return Vector2.zero;
        }
    }
}

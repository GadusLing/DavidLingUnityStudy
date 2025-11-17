using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lesson18 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("鼠标进入对象，触屏上无法触发，因为移动设备没有指针的概念，只有点击");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("鼠标离开对象，触屏上无法触发");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("鼠标(触碰屏幕)按下");
        print(eventData.pointerId);
        print(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("鼠标(触碰屏幕)抬起");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("鼠标点击对象，触屏上为触碰对象后松开，等同于点击");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("开始拖拽对象");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("拖拽中");
        print(eventData.delta);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("结束拖拽对象");
    }

    void Start()
    {
        #region 常用事件接口
        // IPointerEnterHandler - OnPointerEnter - 当指针进入对象时调用（鼠标进入）
        // IPointerExitHandler - OnPointerExit - 当指针退出对象时调用（鼠标离开）
        // IPointerDownHandler - OnPointerDown - 在对象上按下指针时调用（按下）
        // IPointerUpHandler - OnPointerUp - 松开指针时调用（在指针正在点击的游戏对象上调用）
        // IPointerClickHandler - OnPointerClick - 在同一对象上按下再松开指针时调用（点击）
        // IBeginDragHandler - OnBeginDrag - 即将开始拖动时在拖动对象上调用（开始拖拽）
        // IDragHandler - OnDrag - 发生拖动时在拖动对象上调用（拖拽中）
        // IEndDragHandler - OnEndDrag - 拖动完成时在拖动对象上调用（结束拖拽）
        #endregion

        #region 不常用事件接口了解即可
        // IInitializePotentialDragHandler - OnInitializePotentialDrag - 在找到拖动目标时调用，可用于初始化值
        // IDropHandler - OnDrop - 在拖动目标对象上调用
        // IScrollHandler - OnScroll - 当鼠标滚轮滚动时调用
        // IUpdateSelectedHandler - OnUpdateSelected - 每次勾选时在选定对象上调用
        // ISelectHandler - OnSelect - 当对象成为选定对象时调用
        // IDeselectHandler - OnDeselect - 取消选择选定对象时调用
        #endregion

        #region 导航相关
        // IMoveHandler - OnMove - 发生移动事件（上、下、左、右等）时调用
        // ISubmitHandler - OnSubmit - 按下Submit按钮时调用
        // ICancelHandler - OnCancel - 按下Cancel按钮时调用
        #endregion

    }

    void Update()
    {
    }
}

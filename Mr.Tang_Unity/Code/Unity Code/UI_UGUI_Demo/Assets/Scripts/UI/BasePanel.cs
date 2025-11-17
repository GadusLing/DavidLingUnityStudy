using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 整体控制淡入淡出画布组的组件
    /// </summary>
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 淡入淡出速度
    /// </summary>
    private float alphaSpeed = 10f;
    /// <summary>
    /// 是否要显示
    /// </summary>
    private bool isShow;

    /// <summary>
    /// 当自己淡出完成时，要调用的事件(关闭面板等等)
    /// </summary>
    private UnityAction hideCallBack;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 主要用于 初始化按钮的监听事件等等
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 呼出该面板时做什么
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 退出该面板时做什么（主要控制淡出）
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        // 记录当淡出成功后要执行的函数
        hideCallBack = callBack;
    }



    // Update is called once per frame
    void Update()
    {
        // 淡入
        if(isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        // 淡出
        else if(!isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //当透明度到达0，让管理器删除该面板 当HideMe发生时会传一个委托函数过来 通过委托来调用删除函数
                hideCallBack?.Invoke();
            }
        }
    }
}

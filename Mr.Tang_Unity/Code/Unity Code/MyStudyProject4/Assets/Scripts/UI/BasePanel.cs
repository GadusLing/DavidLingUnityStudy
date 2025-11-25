using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 用于控制UI面板整体透明度的组件
    /// </summary>
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 透明度变化速度
    /// </summary>
    private float alphaSpeed = 10f;

    /// <summary>
    /// 当前面板是要显示还是隐藏 默认false隐藏
    /// </summary>
    private bool isShow = false;

    /// <summary>
    /// 隐藏完毕后要做的回调
    /// </summary>
    private UnityAction hideCallBack = null;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 注册控件事件的方法 所有的子面板都需要注册一些事件 必须实现此方法
    /// </summary>
    public abstract void Init ();

    /// <summary>
    /// 显示自己时需要做的事情
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    /// <summary>
    /// 隐藏自己时需要做的事情
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }

    protected virtual void Update()
    {
        // 控制透明度渐变
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;     
        }
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
                {
                    canvasGroup.alpha = 0;
                    // 隐藏完毕后执行回调
                    hideCallBack?.Invoke();
                }
        }
    }

    
}

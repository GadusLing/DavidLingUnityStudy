using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance = new UIManager();
    public static UIManager Instance => _instance;

    /// <summary>
    /// 储存面板信息的字典容器
    /// </summary>
    public Dictionary<String, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    
    private Transform canvasTrans;
    private UIManager()
    {
        // 得到场景上创建好的Canvas对象
        canvasTrans = GameObject.Find("Canvas").transform;
        // 过场景不移除Canvas，在场景上把UICamer和EventSystem也作为了Canvas的子物体，这样在过场景时就不会影响任何UI功能
        GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
    }

    /// <summary>
    /// 显示面板 
    /// </summary>
    /// <typeparam name="T">通过where T : BasePanel约束，只有继承自BasePanel的类才能作为面板类型</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        //我们只需要保证 泛型T的类型和面板名一致 就可以通过typeof(T).Name来获取面板名
        string panelName = typeof(T).Name;

        // 先从字典中查找，如果存在，也就意味面板是显示状态，直接返回该面板即可，否则就会重复创建面板
        if(panelDic.TryGetValue(panelName, out BasePanel existingPanel))
            return existingPanel as T;
        
        // 显示面板，就是动态的创建面板预设体并设置父对象
        // 根据得到的类名就是我们的预设体面板名，直接动态创建它即可
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UIPanelPrefab/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);
        // 接着就是得到对应的面板脚本并存储起来
        T panel = panelObj.GetComponent<T>();
        // 把面板脚本存储到对应容器中之后可以方便我们获取它
        panelDic.Add(panelName, panel);

        // 调用显示自己的逻辑
        panel.ShowMe();
        // 链式编程 返回面板脚本，方便后续操作
        return panel;

    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">通过where T : BasePanel约束，只有继承自BasePanel的类才能作为面板类型</typeparam>
    /// <param name="isFade">默认有淡入淡出，如需立刻关闭请设置为false</param>
    /// <returns></returns>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.TryGetValue(panelName, out BasePanel panel))
        {
            if(isFade)
            {
                // 有淡出的话 调用隐藏自己的逻辑
                panel.HideMe(() =>
                {
                    // 隐藏完成后销毁面板对象并从字典中移除
                    GameObject.Destroy(panel.gameObject);
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                // 立刻销毁面板对象并从字典中移除
                GameObject.Destroy(panel.gameObject);
                panelDic.Remove(panelName);
            }
        }
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.TryGetValue(panelName, out BasePanel panel))
        {
            return panel as T;
        }
        return null;
    }
}

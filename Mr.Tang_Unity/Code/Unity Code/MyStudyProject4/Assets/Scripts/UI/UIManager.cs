using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance = new UIManager();
    public static UIManager Instance => _instance;
    /// <summary>
    /// 存储所有UI面板的字典 每显示一个面板 就往里面添加一个信息 隐藏面板就可以从中获取并隐藏
    /// </summary>
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    /// <summary>
    /// 场景中的Canvas的Transform组件 所有的UI面板都要作为Canvas的子物体
    /// </summary>
    private Transform canvasTransform;
    private UIManager() 
    { 
        GameObject canvasObj = GameObject.Instantiate(Resources.Load<GameObject>($"UI/PanelsPrefabs/Canvas"));
        canvasTransform = canvasObj.transform;
        GameObject.DontDestroyOnLoad(canvasObj); // 确保Canvas在场景切换时不被销毁 保证整个游戏只有一个Canvas
    }



    public T ShowPanel<T>() where T : BasePanel
    {
        // 只需要保证面板类名和预制体名一致 就可以方便加载
        string panelName = typeof(T).Name;

        // 先检查字典中是否已经存在该面板 存在意味着已经显示了该面板 直接返回即可
        if(panelDic.TryGetValue(panelName, out BasePanel panel))
            return panel as T;
    
        // 不存在则加载该面板预制体 并实例化
        GameObject panelPrefab = Resources.Load<GameObject>($"UI/PanelsPrefabs/{panelName}");
        if(panelPrefab == null)
        {
            Debug.LogError($"无法加载名为{panelName}的面板预制体，请检查路径是否正确");
            return null;
        }
        GameObject panelObj = GameObject.Instantiate(panelPrefab, canvasTransform);// 实例化panelPrefab并指定Canvas为父物体
        T Panel = panelObj.GetComponent<T>();// 获取panelObj面板上的脚本组件 一般脚本名类名和预制体名是一样的
        if(Panel == null)
        {
            Debug.LogError($"预制体{panelName}上没有挂载{panelName}脚本，请检查");
            GameObject.Destroy(panelObj);
            return null;
        }
        panelDic.Add(panelName, Panel);// 将成功创建的面板加入字典中
        Panel.ShowMe();
        return Panel;  
    }

    /// <summary>
    /// 隐藏指定类型的面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否使用淡出效果 默认使用</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDic.TryGetValue(panelName, out BasePanel panel))
        {
            if(isFade)
            {
                panel.HideMe(() => 
                {
                    GameObject.Destroy(panel.gameObject);
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                GameObject.Destroy(panel.gameObject);
                panelDic.Remove(panelName);
            }
        }
        else
        {
            Debug.LogWarning($"尝试隐藏名为{panelName}的面板失败，因为该面板不存在");
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDic.TryGetValue(panelName, out BasePanel panel))
        {
            return panel as T;
        }
        else
        {
            Debug.LogWarning($"尝试获取名为{panelName}的面板失败，因为该面板不存在");
            return null;
        }
    }
    
}

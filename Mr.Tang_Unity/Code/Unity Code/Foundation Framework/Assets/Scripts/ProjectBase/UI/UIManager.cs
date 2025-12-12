using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI管理器，负责管理所有UI面板的显示与隐藏
/// </summary>
public class UIManager : SingletonBase<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    public RectTransform canvas;

    public UIManager()
    {
        canvas = ResManager.Instance.LoadRes<GameObject>("UI/Canvas").transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvas.gameObject);
        Transform eventSystem = ResManager.Instance.LoadRes<GameObject>("UI/EventSystem").transform;
        GameObject.DontDestroyOnLoad(eventSystem.gameObject);
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");
    }

    public Transform GetUILayer(E_UI_Layer e_UI_Layer)
    {
        switch (e_UI_Layer)
        {
            case E_UI_Layer.Bot:
                return bot;
            case E_UI_Layer.Mid:
                return mid;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
            default:
                return null;
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名称</param>
    /// <param name="e_UI_Layer">面板所在层级</param>
    /// <param name="callBack">面板创建成功后的回调</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer e_UI_Layer = E_UI_Layer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            callBack?.Invoke(panelDic[panelName] as T);
            return;
        }
        ResManager.Instance.LoadResAsync<GameObject>($"UI/Panels/{panelName}", (panelObj) =>
        {
            Transform father = bot;
            switch (e_UI_Layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            panelObj.transform.SetParent(father, false);
            RectTransform rt = panelObj.transform as RectTransform;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        
            T panel = panelObj.GetComponent<T>();
            callBack?.Invoke(panel);

            panel.ShowMe();

            panelDic.Add(panelName, panel);

        });
    }
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType eventType, UnityAction<BaseEventData> callBack)
    {
        EventTrigger eventTrigger = control.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callBack);
        eventTrigger.triggers.Add(entry);
        // 这里有时间还可以做一个优化 当一个按钮触发时可能会有多个事件监听器 这里可以做一个判断 如果已经有这个事件类型的监听器了就不需要再添加一个新的entry了 直接在原有的entry上添加回调就行
        // 但是实际项目中这种情况比较少见 如果有这样的情况其实是设计层面的问题 所以暂时先不做这个优化了
    }
}

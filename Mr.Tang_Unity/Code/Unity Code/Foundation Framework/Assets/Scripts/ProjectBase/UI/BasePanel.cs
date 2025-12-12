using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI面板的基类，主要目的：
/// 1. 自动查找并存储面板下的所有UI控件（游戏对象如btnStart）。
/// 2. 提供通过控件名获取其上UI组件的方法。
/// 3. 提供显示/隐藏等通用接口。
/// </summary>
public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 存储面板下所有UI控件及其对应的UI组件。
    /// 键 (Key): string 类型，存储UI控件的游戏对象（GameObject）名，例如 "btnStart"。
    /// 值 (Value): List<UIBehaviour> 类型，存储该游戏对象上挂载的所有UI组件（如 Button, Image 等）。
    /// </summary>
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<TMP_Text>();
        FindChildrenControl<Slider>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<TMP_InputField>();
    }

    public virtual void ShowMe()
    {

    }

    public virtual void HideMe()
    {
        
    }

    protected virtual void OnClick(String btnName)
    {
        
    }

    protected virtual void OnValueChanged(String toggleName, bool isOn)
    {
        
    }

    /// <summary>
    /// 通过UI控件的游戏对象名，获取其上挂载的指定类型UI组件。
    /// </summary>
    /// <typeparam name="T">要查找的UI组件类型，如 Button, Image 等。</typeparam>
    /// <param name="controlName">UI控件的游戏对象名 如 "btnStart"</param>
    /// <returns>找到的UI组件，如果未找到则返回null。</returns>
    protected T GetTControl<T>(string controlName) where T : UIBehaviour // 此处其实还能优化，在找到了所有控件时还能顺便添加上事件
    {
        if (controlDic.ContainsKey(controlName))
        {
            foreach (var control in controlDic[controlName])
            {
                if (control is T)
                    return control as T;
            }
        }
        Debug.LogError($"在面板 {this.name} 中，名为 {controlName} 的控件上没有找到 {typeof(T)} 类型的UI组件");
        return null;
    }
    
    /// <summary>
    /// 查找并缓存控件下所有指定类型的UI组件。
    /// </summary>
    /// <typeparam name="T">UI组件类型</typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        // 1. 查找所有子对象（包括自身）上挂载的 T 类型UI组件，返回一个组件数组。
        T[] controls = GetComponentsInChildren<T>();

        // 2. 遍历所有找到的该类型UI组件。例如，如果T是Button，controls就是此面板下所有Button组件的数组。
        foreach (var control in controls)
        {
            // 3. control.name 是该UI组件所在游戏对象的名字（例如 "btnStart"）。如果字典里还没有这个名字的键...
            if (!controlDic.ContainsKey(control.name))
                // ...就以该游戏对象名为键，创建一个新列表。
                controlDic.Add(control.name, new List<UIBehaviour>());
            
            // 4. 将当前UI组件添加到对应游戏对象名的列表中。
            controlDic[control.name].Add(control);
            // 最终，字典中会以控件的游戏对象名为键，存储一个包含其所有UI组件的列表。

            // 5. 如果该UI组件是Button类型，在这里为其添加一个空的OnClick虚函数 这样在子类中就能重写OnClick方法使用
            if(control is Button btn)
            {
                btn.onClick.AddListener(() =>
                {
                    OnClick(control.name);
                });
            }
            // 6. 如果该UI组件是Toggle类型，在这里为其添加一个OnValueChanged虚函数 这样在子类中就能重写OnValueChanged方法使用
            if(control is Toggle toggle)
            {
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    OnValueChanged(control.name, isOn);
                });
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] // 使脚本在编辑模式下也能执行 OnGUI
public class CustomGUIRoot : MonoBehaviour
{
    private CustomGUIControl[] allguiControls;

    void Start()
    {
        allguiControls = this.GetComponentsInChildren<CustomGUIControl>(true);
    }

    private void OnGUI()
    {
        // 获取所有子对象中的 CustomGUIControl 组件
        if (Application.isPlaying == false)// 游戏运行时不要每次都GetComponentsInChildren 影响性能
        {
            allguiControls = this.GetComponentsInChildren<CustomGUIControl>();
        }
        
        if (allguiControls != null && allguiControls.Length > 0)
        {
            foreach (var item in allguiControls)
            {
                item.DrawGUI();
            }
        }
    }

}

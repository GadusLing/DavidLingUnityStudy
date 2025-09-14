using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGUIToggle : CustomGUIControl
{
    public bool isSelected;
    
    private bool isOldSelected;

    public event UnityAction<bool> changeValue;

    protected override void StyleOnDraw()
    {
        isSelected = GUI.Toggle(guiPos.GetRect(), isSelected, guiContent, guiStyle);
        if (isOldSelected != isSelected)// 只有状态改变才触发事件，避免每一帧都重复获取状态
        {
            changeValue?.Invoke(isSelected);
            isOldSelected = isSelected;
        }
    }

    protected override void StyleOffDraw()
    {
        isSelected = GUI.Toggle(guiPos.GetRect(), isSelected, guiContent);
        if (isOldSelected != isSelected)
        {
            changeValue?.Invoke(isSelected);
            isOldSelected = isSelected;
        }
    }
}

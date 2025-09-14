using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGUIToggleGroup : MonoBehaviour
{
    public List<CustomGUIToggle> toggleList = new List<CustomGUIToggle>();

    private CustomGUIToggle frontToggle;
    private void Start()
    {
        if (toggleList == null || toggleList.Count == 0) return;
        
        foreach (var toggle in toggleList)
        {
            CustomGUIToggle currentToggle = toggle;
            toggle.changeValue += (isOn) =>
            {
                if (isOn)// 如果传进来的isOn是true，说明当前Toggle被选中 那就把另外的Toggle都设置为false
                {
                    foreach (var toggle in toggleList)
                    {
                        if (toggle != currentToggle) toggle.isSelected = false;
                    }
                    frontToggle = currentToggle;
                }
                else if(toggle == frontToggle)// 如果传进来的isOn是false，说明当前Toggle被取消选中
                {
                    // 这里是为了防止重复触发事件
                    // 如果前一个Toggle和当前Toggle是同一个 并且当前Toggle被取消选中 那么就把当前Toggle重新设置为选中状态
                    toggle.isSelected = true;
                }
            };
        }
    }
}
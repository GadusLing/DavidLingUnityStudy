using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_Slider_Type
{
    Horizontal,
    Vertical
}

public class CustomGUISlider : CustomGUIControl
{
    public float minValue = 0.5f;
    public float maxValue = 1.0f;
    public float nowValue = 0;

    public E_Slider_Type sliderType = E_Slider_Type.Horizontal;
    public GUIStyle styleThumb;

    public event UnityAction<float> changeValue;

    private float lastValue = 0;

    protected override void StyleOnDraw()
    {
        switch (sliderType)
        {
            case E_Slider_Type.Horizontal:
                nowValue = GUI.HorizontalSlider(guiPos.GetRect(), nowValue, minValue, maxValue, guiStyle, styleThumb);
                break;
            case E_Slider_Type.Vertical:
                nowValue = GUI.VerticalSlider(guiPos.GetRect(), nowValue, minValue, maxValue, guiStyle, styleThumb);
                break;
        }

        if(nowValue != lastValue)
        {
            changeValue?.Invoke(nowValue);
            lastValue = nowValue;
        }
    }

    protected override void StyleOffDraw()
    {
        switch (sliderType)
        {
            case E_Slider_Type.Horizontal:
                nowValue = GUI.HorizontalSlider(guiPos.GetRect(), nowValue, minValue, maxValue);
                break;
            case E_Slider_Type.Vertical:
                nowValue = GUI.VerticalSlider(guiPos.GetRect(), nowValue, minValue, maxValue);
                break;
        }

        if(nowValue != lastValue)
        {
            changeValue?.Invoke(nowValue);
            lastValue = nowValue;
        }
    }
}

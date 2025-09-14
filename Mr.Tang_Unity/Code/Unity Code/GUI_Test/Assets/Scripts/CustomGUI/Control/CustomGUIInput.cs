using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class CustomGUIInput : CustomGUIControl
{
    public event UnityAction<string> textChange;
    private string lastText = "";

    protected override void StyleOnDraw()
    {
        guiContent.text = GUI.TextField(guiPos.GetRect(), guiContent.text, guiStyle);
        if (lastText != guiContent.text)
        {
            textChange?.Invoke(guiContent.text);
            lastText = guiContent.text;
        }
    }

    protected override void StyleOffDraw()
    {
        guiContent.text = GUI.TextField(guiPos.GetRect(), guiContent.text);
        if (lastText != guiContent.text)
        {
            textChange?.Invoke(guiContent.text);
            lastText = guiContent.text;
        }
    }
}

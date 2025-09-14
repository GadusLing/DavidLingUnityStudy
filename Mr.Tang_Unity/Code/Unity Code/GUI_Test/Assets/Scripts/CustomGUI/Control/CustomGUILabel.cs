using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGUILabel : CustomGUIControl
{
    protected override void StyleOnDraw()
    {
        GUI.Label(guiPos.GetRect(), guiContent, guiStyle);
    }

    protected override void StyleOffDraw()
    {
        GUI.Label(guiPos.GetRect(), guiContent);
    }
}

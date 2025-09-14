using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGUITexture : CustomGUIControl
{
    public ScaleMode scaleMode = ScaleMode.StretchToFill;

    protected override void StyleOnDraw()
    {
        GUI.DrawTexture(guiPos.GetRect(), guiContent.image, scaleMode);
    }

    protected override void StyleOffDraw()
    {
        GUI.DrawTexture(guiPos.GetRect(), guiContent.image, scaleMode);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopersPanel : BasePanel<DevelopersPanel>
{
    public CustomGUIButton ButtonClose;

    void Start()
    {
        ButtonClose.clickEvent += () =>
        {
            HidePanel();
            BeginPanel.Instance.ShowPanel();
        };
        HidePanel();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }
}


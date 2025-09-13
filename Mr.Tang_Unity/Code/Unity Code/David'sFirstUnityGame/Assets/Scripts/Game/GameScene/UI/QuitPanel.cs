using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPanel : BasePanel<QuitPanel>
{
    public CustomGUIButton QuitButton;
    public CustomGUIButton ContinueButton;

    void Start()
    {
        QuitButton.clickEvent += () =>
        {
            SceneManager.LoadScene("BeginScene");
        };
        ContinueButton.clickEvent += () =>
        {
            HidePanel();
        };
        HidePanel();
    }

    public override void HidePanel()
    {
        base.HidePanel();
        Time.timeScale = 1;
    }
}

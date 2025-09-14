using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatPanel : BasePanel<DefeatPanel>
{
    public CustomGUIButton buttonQuit;
    public CustomGUIButton buttonContinue;

    void Start()
    {
        buttonQuit.clickEvent += () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("BeginScene");
        };

        buttonContinue.clickEvent += () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GamingScene");
        };
        HidePanel();
    }
}

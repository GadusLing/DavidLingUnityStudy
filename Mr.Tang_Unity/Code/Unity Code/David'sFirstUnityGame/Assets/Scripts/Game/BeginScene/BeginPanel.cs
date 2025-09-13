using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginPanel : BasePanel<BeginPanel>
{
    public CustomGUIButton ButtonStart;
    public CustomGUIButton ButtonSetting;
    public CustomGUIButton ButtonDevelopers;
    public CustomGUIButton ButtonQuit;
    public CustomGUIButton ButtonRankings;


    
    void Start()
    {
        ButtonStart.clickEvent += () =>
        {
            SceneManager.LoadScene("GamingScene");
        };
        ButtonSetting.clickEvent += () =>
        {
            SettingsPanel.Instance.ShowPanel();
            HidePanel();
        };
        ButtonDevelopers.clickEvent += () =>
        {
            DevelopersPanel.Instance.ShowPanel();
            HidePanel();
        };
        ButtonQuit.clickEvent += () =>
        {
            Application.Quit();
        };
        ButtonRankings.clickEvent += () =>
        {
            RankingsPanel.Instance.ShowPanel();
            HidePanel();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

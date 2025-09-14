using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : BasePanel<WinPanel>
{
    public CustomGUIInput inputInfo;
    public CustomGUIButton buttonConfirm;


    void Start()
    {
        buttonConfirm.clickEvent += () =>
        {
            Time.timeScale = 1;
            GameDataManager.Instance.AddRankInfo(inputInfo.guiContent.text, 
            GamePanel.Instance.currentScore, 
            GamePanel.Instance.currentTime);

            SceneManager.LoadScene("BeginScene");
        };
        HidePanel();
    }
}

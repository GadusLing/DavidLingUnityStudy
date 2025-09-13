using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class GamePanel : BasePanel<GamePanel>
{
    public CustomGUILabel ScoreLabel;
    public CustomGUILabel TimeLabel;
    public CustomGUIButton QuitButton;
    public CustomGUIButton SettingsButton;
    public CustomGUITexture HPTexture;

    public float HPWidth = 800;

    [HideInInspector]
    public int currentScore;

    
    [HideInInspector]
    public float currentTime;


    void Start()
    {
        SettingsButton.clickEvent += () =>
        {
            SettingsPanel.Instance.ShowPanel();
            Time.timeScale = 0;
        };

        QuitButton.clickEvent += () =>
        {
            QuitPanel.Instance.ShowPanel();
            Time.timeScale = 0;
        };
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;//通过帧间隔时间累加比较准确
        int time = (int)currentTime;
        int hour = time / 3600;
        int minute = (time % 3600) / 60;    
        int second = time % 60;
        TimeLabel.guiContent.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
    }

    public void UpdateScore(int score)
    {
        currentScore += score;
        ScoreLabel.guiContent.text = currentScore.ToString();
    }

    public void UpdateHP(int maxHP, int HP)
    {
        HPTexture.guiPos.size.x = (float)HP / maxHP * HPWidth;
    }

}
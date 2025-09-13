using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanel : BasePanel<SettingsPanel>
{
    public CustomGUISlider SliderMusic;
    public CustomGUISlider SliderSound;
    public CustomGUIToggle ToggleMusic;
    public CustomGUIToggle ToggleSound;
    public CustomGUIButton ButtonClose;

    void Start()
    {
        SliderMusic.changeValue += (value) => GameDataManager.Instance.ChangeMusicVolume(value);
        SliderSound.changeValue += (value) => GameDataManager.Instance.ChangeSoundVolume(value);
        ToggleMusic.changeValue += (isOn) => GameDataManager.Instance.OnOffMusic(isOn);
        ToggleSound.changeValue += (isOn) => GameDataManager.Instance.OnOffSound(isOn);
            
        ButtonClose.clickEvent += () =>
        {
            HidePanel();

            if (SceneManager.GetActiveScene().name.Equals("BeginScene")) BeginPanel.Instance.ShowPanel();
        };
        HidePanel();
    }

    public void UpdatePanelInfo()
    {
        MusicData data = GameDataManager.Instance.musicData;
        SliderMusic.nowValue = data.musicVolume;
        SliderSound.nowValue = data.soundVolume;
        ToggleMusic.isSelected = data.isMusicOn;
        ToggleSound.isSelected = data.isSoundOn;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        UpdatePanelInfo();
    }

    public override void HidePanel()
    {
        base.HidePanel();
        Time.timeScale = 1;
    }
}

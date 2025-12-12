using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSoundTest : MonoBehaviour
{
    float v;
    AudioSource currentSound;
    void OnGUI()
    {
        if(GUI.Button(new Rect(100,100,200,100),"Play BGM"))
        {
            MusicSoundManager.Instance.PlayBGM("Black");
        }
        if (GUI.Button(new Rect(100, 250, 200, 100), "Pause BGM"))
        {
            MusicSoundManager.Instance.PauseBGM();
        }
        if (GUI.Button(new Rect(100, 400, 200, 100), "Stop BGM"))
        {
            MusicSoundManager.Instance.StopBGM();
        }
        v = GUI.HorizontalSlider(new Rect(100, 550, 200, 100), v, 0.0f, 1.0f);
        MusicSoundManager.Instance.SetBGMVolume(v);

        if (GUI.Button(new Rect(400, 100, 200, 100), "Play Sound"))
        {
            MusicSoundManager.Instance.PlaySound("ReloadSound", false, (source) =>
            {
                // 可以在这里对播放的音效source进行一些操作 比如记录到列表中以便后续停止播放等
                currentSound = source;
            });
        }
        if (GUI.Button(new Rect(400, 250, 200, 100), "Stop Sound"))
        {
            if(currentSound != null)
            {
                MusicSoundManager.Instance.StopSound(currentSound);
                currentSound = null;
            }
        }

    }

}

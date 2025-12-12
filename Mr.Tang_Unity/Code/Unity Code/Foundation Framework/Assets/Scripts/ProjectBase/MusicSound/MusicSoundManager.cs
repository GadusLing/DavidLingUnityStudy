using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicSoundManager : SingletonBase<MusicSoundManager>
{
    private AudioSource bgmSource = null;
    private float bgmVolume = 0.5f;

    private GameObject soundParent = null;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float soundVolume = 0.5f;

    /// <summary>
    /// AudioSource控制音效有个最大的问题就是它没有任何方式，包括回调包括事件，来告诉你音效播放完毕了
    /// 所以只能用轮询的方式来检查音效是否播放完毕 要用到轮询的话 就需要一个Update函数
    /// 而没有继承MonoBehaviour的单例类是没有Update函数的 所以采用MonoManager来帮助其实现Update轮询
    /// </summary>
    public MusicSoundManager()
    {
        MonoManager.Instance.AddUpdateListener(Update);
    }

    private void Update()
    {
        for(int i = soundList.Count - 1; i >= 0; i--)
        {
            if(!soundList[i].isPlaying)// 此处也可用缓存池优化
            {
                Object.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    public void PlayBGM(string bgmName)
    {
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGMSource");
            bgmSource = bgmObj.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(bgmObj);
        }

        ResManager.Instance.LoadResAsync<AudioClip>("Music/BGM/" + bgmName, (clip) =>
        {
            bgmSource.clip = clip;
            bgmSource.volume = bgmVolume;
            bgmSource.loop = true;
            bgmSource.Play();
        });
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }

    public void PauseBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Pause();
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    public void PlaySound(string soundName, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if(soundParent == null)
        {
            soundParent = new GameObject("SoundParent");
            Object.DontDestroyOnLoad(soundParent);
        }

        ResManager.Instance.LoadResAsync<AudioClip>("Music/Sound/" + soundName, (clip) =>
        {
            AudioSource soundSource = soundParent.AddComponent<AudioSource>();
            soundSource.clip = clip;
            soundSource.loop = isLoop;
            soundSource.volume = soundVolume;
            soundSource.Play();
            soundList.Add(soundSource);
            if(callback != null)
            {
                callback(soundSource);
            }
        });
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach(var sound in soundList)
        {
            sound.volume = soundVolume;
        }
    }

    // 后续可以用缓存池优化
    public void StopSound(AudioSource soundName)
    {
        if(soundList.Contains(soundName))
        {
            soundName.Stop();
            soundList.Remove(soundName);
            Object.Destroy(soundName);
        }
    }
}

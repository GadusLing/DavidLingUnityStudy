using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMandSound : MonoBehaviour
{
    private static BGMandSound instance;
    public static BGMandSound Instance => instance ?? (instance = new BGMandSound());

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
        changeValue(GameDataManager.Instance.musicData.musicVolume);
        OnOffMusic(GameDataManager.Instance.musicData.isMusicOn);
    }

    public void changeValue(float volume)
    {
        audioSource.volume = volume;
    }
    public void OnOffMusic(bool isOn)
    {
        audioSource.mute = !isOn;
    }
}

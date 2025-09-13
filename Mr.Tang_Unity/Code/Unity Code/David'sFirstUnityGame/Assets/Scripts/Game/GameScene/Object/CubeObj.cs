using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeObj : MonoBehaviour
{
    public GameObject[] rewardPrefabs;

    public GameObject DeadEffect;
    private Transform anchor;

    private void Start()
    {
        anchor = transform.Find("Anchor");
    }

    private void OnTriggerEnter(Collider other)
    {
        int rangeInt = Random.Range(0, 100);
        if(rangeInt < 65) // 65%概率生成奖励
        {
            rangeInt = Random.Range(0, rewardPrefabs.Length);
            Vector3 spawnPos = anchor != null ? anchor.position : transform.position;
            Instantiate(rewardPrefabs[rangeInt], spawnPos, Quaternion.identity);
        }

        if(DeadEffect != null)
        {
            GameObject eff = Instantiate(DeadEffect, transform.position, Quaternion.identity);
            AudioSource audioS = eff.AddComponent<AudioSource>();
            audioS.volume = GameDataManager.Instance.musicData.soundVolume;
            audioS.mute = !GameDataManager.Instance.musicData.isSoundOn;
        }
        Destroy(gameObject);
    }

}

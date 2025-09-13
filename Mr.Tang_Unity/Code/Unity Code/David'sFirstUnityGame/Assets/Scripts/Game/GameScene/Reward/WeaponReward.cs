using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReward : MonoBehaviour
{
    public GameObject[] weaponObjs;
    public GameObject getEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int randomIndex = Random.Range(0, weaponObjs.Length);
            PlayerObj player = other.GetComponent<PlayerObj>();
            player.ChangeBarrel(weaponObjs[randomIndex]);
            GameObject eff = Instantiate(getEffect, transform.position, Quaternion.identity);
            AudioSource audioS = eff.AddComponent<AudioSource>();
            audioS.volume = GameDataManager.Instance.musicData.soundVolume;
            audioS.mute = !GameDataManager.Instance.musicData.isMusicOn;
            Destroy(gameObject);
        }
    }
}

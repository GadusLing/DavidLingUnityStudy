using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TankBaseObj : MonoBehaviour
{
    public int atk;
    public int def;
    public int maxHP;
    public int HP;
    public Transform Turret;

    public float moveSpeed = 10f;
    public float rotateSpeed = 100f;
    public float headRoundSpeed = 100f;

    public GameObject DeadEffect;

    public abstract void Attack();
    public virtual void BeAttacked(TankBaseObj attacker)
    {
        int damage = attacker.atk - def;
        if (damage > 0) 
        {
            HP -= damage;
            if (HP <= 0) Die();
        }
    }
    public virtual void Die()
    {
        Destroy(gameObject);
        if (DeadEffect != null) 
        {
            GameObject effObj = Instantiate(DeadEffect, transform.position, Quaternion.identity);
            AudioSource audioSource = effObj.GetComponent<AudioSource>();
            audioSource.volume = GameDataManager.Instance.musicData.soundVolume;
            audioSource.mute = !GameDataManager.Instance.musicData.isSoundOn;
            audioSource.Play();
        }
    }
}

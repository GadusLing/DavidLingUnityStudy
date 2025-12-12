using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Goblin";
    void Start()
    {
        Invoke("Dead", 1f);
    }
    void Dead()
    {
        Debug.Log($"{enemyName} 死了");
        EventCenter.Instance.EventTrigger("EnemyDead", enemyName);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTower : TankBaseObj
{
    public float AttackTime = 1;
    private float currentTime = 0;

    public Transform[] shootPos;

    public GameObject bulletObj;

    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= AttackTime) 
        {
            Attack();
            currentTime = 0;
        }
    }
    public override void Attack()
    {
        for(int i = 0; i < shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bulletObj, shootPos[i].position, shootPos[i].rotation);
            BulletObj bullet = obj.GetComponent<BulletObj>();
            bullet.SetOwner(this);
        }
    }


    public override void BeAttacked(TankBaseObj attacker)
    {
        // 这种类型的敌人不会被伤害，这里重写后不写内容
    }
}

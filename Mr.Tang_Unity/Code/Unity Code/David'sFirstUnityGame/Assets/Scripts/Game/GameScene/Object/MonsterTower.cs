using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTower : TankBaseObj
{
    public float AttackTime = 1;
    private float currentTime = 0;

    public Transform[] shootPos;

    public GameObject bulletObj;

    // Start is called before the first frame update
    void Start()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= AttackTime) Attack();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Attack()
    {
        foreach(var bulltpos in shootPos)
        {
            GameObject obj = Instantiate(bulletObj, bulltpos.position, Quaternion.identity);
            BulletObj bullet = obj.AddComponent<BulletObj>();
            bullet.SetOwner(this);
        }
    }
}

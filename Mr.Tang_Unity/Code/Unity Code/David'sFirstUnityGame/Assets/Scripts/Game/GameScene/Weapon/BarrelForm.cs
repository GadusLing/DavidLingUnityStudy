using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelForm : MonoBehaviour
{
    public GameObject bullet;

    // 如果有多炮口武器这里炮口用数组，我只有一个炮口，用一个变量即可
    public Transform shootPoint;

    // 计算子弹造成的伤害必须要持有者信息
    public TankBaseObj owner;

    //设置持有者
    public void SetOwner(TankBaseObj newOwner)
    {
        owner = newOwner;
    }

    public void Fire()
    {
        // 创建子弹预设体
        GameObject obj = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        // 控制子弹
        BulletObj bulletObj = obj.GetComponent<BulletObj>();
        bulletObj.owner = owner;
    }
}

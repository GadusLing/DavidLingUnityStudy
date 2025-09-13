using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerObj : TankBaseObj
{
    //当前坦克的炮管，影响开火和子弹类型
    public BarrelForm currentBarrel;

    public Transform TurretTransform;

    void Update()
    {
        transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * moveSpeed * Time.deltaTime);
        transform.Rotate(Input.GetAxis("Horizontal") * Vector3.up * rotateSpeed * Time.deltaTime);
        Turret.transform.Rotate(Input.GetAxis("Mouse X") * Vector3.up * headRoundSpeed * Time.deltaTime);
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (currentBarrel != null)
        {
            currentBarrel.Fire();
        }
    }

    public override void BeAttacked(TankBaseObj attacker)
    {
        base.BeAttacked(attacker);
        GamePanel.Instance.UpdateHP(maxHP, HP);
    }

    public override void Die()
    {
        base.Die();
    }

    public void ChangeBarrel(GameObject newBarrel)
    {
        if(currentBarrel != null)
        {
            Destroy(currentBarrel.gameObject);
            currentBarrel = null;
        }
        GameObject newBarrelInstance = Instantiate(newBarrel, TurretTransform, false);
        currentBarrel = newBarrelInstance.GetComponent<BarrelForm>();
        currentBarrel.SetOwner(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    private BulletInfo bulletInfo;
    public void InitInfo(BulletInfo info)
    {
        bulletInfo = info;
        //Destroy(gameObject, bulletInfo.lifeTime); // 这种方式不安全，如果在销毁之前就已经碰撞销毁了，二次释放会报错
        Invoke("delaydeath", bulletInfo.lifeTime);  // 所以采用延迟函数的方式，如果当前子弹已经销毁了，就不会再调用这个函数
    }

    private void delaydeath()
    {
        Dead();
    }

    public void Dead()
    {
        // 播放子弹死亡特效
        GameObject effect = Instantiate(Resources.Load<GameObject>(bulletInfo.deadEffect), transform.position, Quaternion.identity);
        Destroy(effect, 1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerObject>().Wound();
            Dead();
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletInfo.forwardSpeed * Time.deltaTime);
        switch(bulletInfo.type)// 1 超面前直线 2 曲线 3 右抛物线 4 左抛物线 5 追踪
        {
            case 2:
                transform.Translate(Vector3.right * Mathf.Sin(Time.time * 5) * bulletInfo.rightSpeed * Time.deltaTime);
                break;
            case 3:
                transform.rotation *= Quaternion.AngleAxis(bulletInfo.rotateSpeed * Time.deltaTime, Vector3.up);
                break;
            case 4:
                transform.rotation *= Quaternion.AngleAxis(-bulletInfo.rotateSpeed * Time.deltaTime, Vector3.up);
                break;
            case 5:
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(
                    PlayerObject.Instance.transform.position - transform.position), Time.deltaTime * bulletInfo.rotateSpeed);
                break;
        }
    }
}

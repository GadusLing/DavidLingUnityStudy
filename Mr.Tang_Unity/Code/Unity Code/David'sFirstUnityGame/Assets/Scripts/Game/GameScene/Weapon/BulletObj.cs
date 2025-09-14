using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObj : MonoBehaviour
{
    public float BulletSpeed = 50f;
    public GameObject hitEffect; // 碰撞特效

    public TankBaseObj owner; // 持有者，谁发射的子弹

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
{
    // Unity 特殊情况：Destroy 后的对象 != null，但 .gameObject 会是 null
    if (other == null || other.gameObject == null) return; 

    // 防御式写法：逐个判断
    bool hitIndestructible = other.gameObject.CompareTag("Indestructible");
    bool hitReward         = other.gameObject.CompareTag("Reward");
    bool hitPlayer         = (owner != null && owner.CompareTag("Enemy") && other.gameObject.CompareTag("Player"));
    bool hitEnemy          = (owner != null && owner.CompareTag("Player") && other.gameObject.CompareTag("Enemy"));

    if (hitIndestructible || hitReward || hitPlayer || hitEnemy)
    {
        TankBaseObj obj = other.GetComponent<TankBaseObj>();
        if (obj != null)
        {
            obj.BeAttacked(owner); // 通知被攻击
        }

        // 播放特效
        if (hitEffect != null)
        {
            GameObject eff = Instantiate(hitEffect, transform.position, Quaternion.identity);
            if (eff.TryGetComponent(out AudioSource audioS))
            {
                audioS.volume = GameDataManager.Instance.musicData.soundVolume; 
                audioS.mute   = !GameDataManager.Instance.musicData.isSoundOn;
            }
        }

        Destroy(gameObject); // 最后销毁子弹
    }
}


    public void SetOwner(TankBaseObj newOwner)
    {
        owner = newOwner;
    }
}

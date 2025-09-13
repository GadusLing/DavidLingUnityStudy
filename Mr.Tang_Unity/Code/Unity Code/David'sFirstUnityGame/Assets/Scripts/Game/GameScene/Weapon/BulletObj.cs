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
        if(other.CompareTag("Indestructible") || other.CompareTag("Reward")) // 碰到不可摧毁物体或玩家
        {
            //销毁时播放特效
            if (hitEffect != null)
            {
                GameObject eff = Instantiate(hitEffect, transform.position, Quaternion.identity);
                AudioSource audioS = eff.GetComponent<AudioSource>();
                audioS.volume = GameDataManager.Instance.musicData.soundVolume; 
                audioS.mute = !GameDataManager.Instance.musicData.isSoundOn;
            }
            Destroy(this.gameObject);
        }
    }

    public void SetOwner(TankBaseObj newOwner)
    {
        owner = newOwner;
    }
}

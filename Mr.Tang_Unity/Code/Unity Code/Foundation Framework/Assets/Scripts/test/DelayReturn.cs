using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayReturn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnEnable()
    {
        Invoke("Return", 1f);
    }

    void Return()
    {
        string key = this.gameObject.name; // 根据物体名称设置 key
        PoolManager.Instance.ReturnObjToPool(key, this.gameObject);
    }
}

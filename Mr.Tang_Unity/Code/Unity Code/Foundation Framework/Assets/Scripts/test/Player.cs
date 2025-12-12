using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.AddEventListener("EnemyDead", EnemyDeadDo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeadDo(object info)
    {
        Debug.Log("玩家收到敌人死亡事件，信息：" + info + "，玩家得到奖励！");
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("EnemyDead", EnemyDeadDo);
    }
}

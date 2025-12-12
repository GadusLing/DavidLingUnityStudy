using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other : MonoBehaviour
{
    void Start()
    {
        EventCenter.Instance.AddEventListener("EnemyDead", OtherWaitEnemyDeadDo);
    }

    public void OtherWaitEnemyDeadDo(object info)
    {
        Debug.Log("其他系统收到敌人死亡事件，信息：" + info);
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("EnemyDead", OtherWaitEnemyDeadDo);
    }
}

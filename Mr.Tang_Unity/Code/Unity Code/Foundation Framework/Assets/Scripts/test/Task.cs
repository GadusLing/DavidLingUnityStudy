using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.AddEventListener("EnemyDead", TaskWaitEnemyDeadDo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TaskWaitEnemyDeadDo(object info)
    {
        Debug.Log("任务系统收到敌人死亡事件，信息：" + info + "，任务进度更新！");
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("EnemyDead", TaskWaitEnemyDeadDo);
    }
}

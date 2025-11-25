using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    public int maxWave;
    public int enemyPerWave;
    private int curNumber;
    public List<int> enemyIDList;
    private int curID;
    public float createInterval;
    public float waveInterval;
    public float firstWaveDelay;
    void Start()
    {
        Invoke(nameof(CreateWave), firstWaveDelay);
        GameLevelMgr.Instance.AddEnemyPoint(this);
        GameLevelMgr.Instance.UpdateMaxWaveNum(maxWave);
    }

    private void CreateWave()
    {
        curID = enemyIDList[Random.Range(0, enemyIDList.Count)];
        curNumber = enemyPerWave;
        CreateEnemy();

        --maxWave;
        GameLevelMgr.Instance.ChangeCurWaveNum();
    }

    private void CreateEnemy()
    {
        EnemyInfo info = GameDataMgr.Instance.enemyInfoList[curID - 1];
        GameObject Obj = Instantiate(Resources.Load<GameObject>(info.prefabRes), transform.position, Quaternion.identity);
        EnemyObject enemyObj = Obj.AddComponent<EnemyObject>();
        enemyObj.InitInfo(info);

        // 为了统计场景种存活的怪物，在创建时，会在GameLevelMgr中增加数量，怪物死亡时减少数量
        //GameLevelMgr.Instance.ChangeEnemyNum();
        GameLevelMgr.Instance.AddEnemyObject(enemyObj);

        --curNumber;
        if (curNumber > 0)
        {
            Invoke(nameof(CreateEnemy), createInterval);
        }
        else if (maxWave > 0)
        {
            Invoke(nameof(CreateWave), waveInterval);
        }
    }

    public bool CheckOver()
    {
        return maxWave <= 0 && curNumber <= 0;
    }
}

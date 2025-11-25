using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameLevelMgr
{
    private static GameLevelMgr _instance = new GameLevelMgr();
    public static GameLevelMgr Instance => _instance;
    private GameLevelMgr() {}

    public PlayerObject player;
    
    /// <summary>
    /// 记录所有出怪点 方便后续判断场上的怪物存活情况 进而判断胜利or失败
    /// </summary>
    private List<EnemyPoint> enemyPointList = new List<EnemyPoint>();

    /// <summary>
    /// 记录当前还有多少波敌人
    /// </summary>
    private int curWaveNum = 0;
    /// <summary>
    /// 记录全局最大波数
    /// </summary>
    private int maxWaveNum = 0;
    // /// <summary>
    // /// 记录当前场景上还有多少只敌人
    // /// </summary>
    // private int curEnemyNum = 0;

    /// <summary>
    /// 记录当前场景上所有敌人的列表
    /// </summary>
    private List<EnemyObject> enemyObjectList = new List<EnemyObject>();

    public void InitInfo(SceneInfo sceneInfo)
    {
        // 显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();
        // 创建玩家
        RoleInfo roleInfo = GameDataMgr.Instance.curChooseRole;
        // 获取玩家出生位置
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        // 实例化玩家对象
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.PrefabRes), heroPos.position, heroPos.rotation);
        // 对玩家对象初始化
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, sceneInfo.sceneGold);
        // 设置摄像机看向对象
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);
        // 初始化主塔血量
        MainTowerObject.Instance.UpdateHP(sceneInfo.mainTowerHP, sceneInfo.mainTowerHP);
    }

    public void AddEnemyPoint(EnemyPoint point)
    {
        enemyPointList.Add(point);
    }

    /// <summary>
    /// 更新全局最大波数
    /// </summary>
    /// <param name="maxWave"></param>
    public void UpdateMaxWaveNum(int maxWave)
    {
        maxWaveNum += maxWave;
        curWaveNum += maxWave; 
        
        // 更新游戏界面波数显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(curWaveNum, maxWaveNum);
    }

    /// <summary>
    /// 更新当前波数
    /// </summary>
    public void ChangeCurWaveNum(int num = 1)
    {
        curWaveNum -= num;
        // 更新游戏界面波数显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(curWaveNum, maxWaveNum);
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        foreach (EnemyPoint point in enemyPointList)
        {
            if (!point.CheckOver())
                return false;
        }
        if (enemyObjectList.Count > 0)
            return false;
        return true;
    }

    // /// <summary>
    // /// 改变当前场景上的敌人数量统计
    // /// </summary>
    // /// <param name="num"></param>
    // public void ChangeEnemyNum(int num = 1)
    // {
    //     curEnemyNum += num;
    // }

    /// <summary>
    /// 添加敌人对象到列表
    /// </summary>
    /// <param name="enemyObj"></param>
    public void AddEnemyObject(EnemyObject enemyObj)
    {
        enemyObjectList.Add(enemyObj);
    }
    /// <summary>
    /// 从列表移除敌人对象
    /// </summary>
    /// <param name="enemyObj"></param>
    public void RemoveEnemyObject(EnemyObject enemyObj)
    {
        enemyObjectList.Remove(enemyObj);
    }

    /// <summary>
    /// 查找指定范围内的单体敌人对象 当敌人存活且塔和敌人距离小于等于塔的攻击范围时 返回该敌人对象给外部去调攻击函数 否则返回null
    /// </summary>
    /// <param name="pos">塔位置</param>
    /// <param name="range">塔的攻击范围</param>
    /// <returns></returns>
    public EnemyObject FindEnemy(Vector3 pos, float range)
    {
        foreach (EnemyObject enemy in enemyObjectList)
        {
            if (enemy.isDead == false && Vector3.Distance(pos, enemy.transform.position) <= range)
            {
                return enemy;
            }
        }
        return null;
    }
    /// <summary>
    /// 查找指定范围内的多个敌人对象 用于范围伤害检测 当敌人存活且塔和敌人距离小于等于塔的攻击范围时 把该敌人对象添加到返回列表中
    /// </summary>
    /// <param name="pos">塔位置</param>
    /// <param name="range">塔的攻击范围</param>
    /// <returns></returns>
    public List<EnemyObject> FindEnemyList(Vector3 pos, float range)
    {
        List<EnemyObject> tempList = new List<EnemyObject>();
        foreach (EnemyObject enemy in enemyObjectList)
        {
            if (enemy.isDead == false && Vector3.Distance(pos, enemy.transform.position) <= range)
            {
                tempList.Add(enemy);
            }
        }
        return tempList;
    }

    /// <summary>
    /// 清空关卡信息，方便下次关卡使用，避免之前的数据影响
    /// </summary>
    public void ClearLevelInfo()
    {
        enemyPointList.Clear();
        curWaveNum = 0;
        maxWaveNum = 0;
        enemyObjectList.Clear();
        player = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    /// <summary>
    /// 炮塔头部的 Transform 组件 控制塔的旋转
    /// </summary>
    public Transform towerHead;
    /// <summary>
    /// 开火点
    /// </summary>
    public Transform firePoint;
    /// <summary>
    /// 旋转速度 先写死
    /// </summary>
    private float rotateSpeed = 50f;
    /// <summary>
    /// 该对象关联的塔信息 之后通过初始化把对应配置数据传进来 就可以通过info.xxx使用了
    /// </summary>
    private TowerInfo info;
    /// <summary>
    /// 炮塔当前攻击目标
    /// </summary>
    private EnemyObject targetObj;
    /// <summary>
    /// 炮塔攻击的敌人列表（群体攻击时使用）
    /// </summary>
    private List<EnemyObject> targetObjList = new List<EnemyObject>();
    /// <summary>
    /// 炮塔攻击计时器 当前计时器用于控制攻击频率 当当前计时器大于攻击间隔时 就开火
    /// </summary>
    private float curTimer = 0f;
    /// <summary>
    /// 记录敌人位置 炮塔转向时需要看向的是敌人胸口的位置，相对于敌人Transform.position有一个Y轴的偏移
    /// </summary>
    private Vector3 EnemyPos;

    public void InitInfo(TowerInfo towerInfo)
    {
        info = towerInfo;
    }

    // Update is called once per frame
    void Update()
    {
        if(info.atkType == 1)// 单体攻击
        {
            // 没有目标or目标死亡or超出攻击范围
            if(targetObj == null || targetObj.isDead || Vector3.Distance(targetObj.transform.position, transform.position) > info.atkRange)
            {
                // 寻找目标 这次我们不使用范围检测 而是在敌人创建时记录下所有敌人对象 然后遍历这些敌人对象 找到一个符合条件的作为目标
                targetObj = GameLevelMgr.Instance.FindEnemy(transform.position, info.atkRange);
            }
            // 有目标 旋转炮塔朝向目标
            if(targetObj != null)
            {
                // 计算敌人位置 炮塔转向时需要看向的是敌人胸口的位置，相对于敌人Transform.position有一个Y轴的偏移
                EnemyPos = targetObj.transform.position;
                EnemyPos.y = towerHead.position.y;
                // 计算目标方向
                //Vector3 dir = (EnemyPos - towerHead.position).normalized;
                // 计算目标旋转 四元数
                Quaternion lookRot = Quaternion.LookRotation(EnemyPos - towerHead.position);
                // 平滑旋转到目标方向
                towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRot, rotateSpeed * Time.deltaTime);

                // 当炮塔头与敌人方向夹角小于5度 并且达到攻击间隔时 开火
                if(Vector3.Angle(EnemyPos - towerHead.position, towerHead.forward) < 5f && Time.time - curTimer >= info.atkInterval)
                {
                    // 让目标受伤 通常还需要射线检测，但这个游戏在怪物和塔之间没有障碍物 所以省略这步 当进入范围并且炮塔角度正确就让怪物受伤
                    targetObj.TakeDamage(info.atk);
                    // 播放攻击动画 特效 音效 子弹等
                    GameDataMgr.Instance.PlaySound("fire");
                    GameObject effObj = Instantiate(Resources.Load<GameObject>(info.atkEff), firePoint.position, firePoint.rotation);
                    Destroy(effObj, 0.2f);// 0.2秒后销毁特效对象
                    // 重置攻击间隔
                    curTimer = Time.time;
                }
            }
        }
        else if(info.atkType == 2)// 群体攻击
        {
            targetObjList = GameLevelMgr.Instance.FindEnemyList(transform.position, info.atkRange);
            // 有目标 并且达到攻击间隔时 攻击 不需要旋转炮塔头
            if(targetObjList.Count > 0 && Time.time - curTimer >= info.atkInterval)
            {
                foreach(EnemyObject enemy in targetObjList)
                {
                    // 让目标们受伤
                    enemy.TakeDamage(info.atk);
                }
                // 播放攻击动画 特效 音效 子弹等
                GameDataMgr.Instance.PlaySound("fire");
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.atkEff), transform.position, transform.rotation);
                Destroy(effObj, 0.2f);// 0.2秒后销毁特效对象
                // 重置攻击间隔
                curTimer = Time.time;
            }
        }
    }
}

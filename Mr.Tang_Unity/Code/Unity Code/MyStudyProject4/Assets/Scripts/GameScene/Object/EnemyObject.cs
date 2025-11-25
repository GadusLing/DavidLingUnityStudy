using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{   
    /// <summary>
    /// 敌人身上的动画组件 负责控制用何动画控制器 播放什么动画
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 敌人身上的导航组件 负责控制敌人的寻路和移动
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 敌人初始在场景里的配置信息
    /// </summary>
    private EnemyInfo enemyInfo;
    /// <summary>
    /// 敌人当前生命值
    /// </summary>
    private int curHp;
    /// <summary>
    /// 敌人是否已经死亡
    /// </summary>
    public bool isDead = false;
    /// <summary>
    /// 攻击计时器 用于控制攻击间隔
    /// </summary>
    private float atkTimer = 0f;
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 根据传入的敌人配置信息 初始化敌人对象
    /// </summary>
    /// <param name="info">敌人配置信息</param>
    public void InitInfo(EnemyInfo info)
    {
        enemyInfo = info;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        curHp = enemyInfo.hp;
        // 速度和加速度 之所以赋值是一样的 是希望没有一个明显的初始加速运动 而是 一个匀速运动初始化 加速度太小的话 初始就有一个很明显的加速过程
        agent.speed = enemyInfo.moveSpeed;
        agent.acceleration = enemyInfo.moveSpeed;

        agent.angularSpeed = enemyInfo.rotateSpeed;
    }

    /// <summary>
    /// 敌人受伤及后续处理 要不要死亡
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;
        
        curHp -= damage;
        animator.SetTrigger("Wound");
        if (curHp <= 0)
        {
            curHp = 0;
            Dead();
        }
        else
        {
            // 播放受伤特效或音效
            GameDataMgr.Instance.PlaySound("enemyHit");
        }
    }

    public void Dead()
    {
        isDead = true;
        agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Dead", true);
        // 播放死亡音效
        GameDataMgr.Instance.PlaySound("dead");
        // 给玩家奖励金币
        GameLevelMgr.Instance.player.AddGold(50); // 每个怪物死亡奖励50金币
    }

    // 死亡动画播放完毕后的事件调用 Event已在动画上添加
    public void DeadEvent()
    {
        // 为了统计场景种存活的怪物，在创建时，会在GameLevelMgr中增加数量，怪物死亡时减少数量
        //GameLevelMgr.Instance.ChangeEnemyNum(-1);
        GameLevelMgr.Instance.RemoveEnemyObject(this);
        // 在场景种移除已经死亡的敌人对象
        Destroy(gameObject);
        // 每当怪物死亡时，都要检查场上还有没有怪物 从而判断当前关卡是否胜利
        if(GameLevelMgr.Instance.CheckOver())
        {
            // 胜利 给玩家奖励金币
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.Gold, true); // 胜利 全部金币奖励
        }
    }

    // 出生动画播放完毕后的事件调用 Event已在动画上添加
    public void BornOver()
    {
        agent.isStopped = false;
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        animator.SetBool("Run", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return;
        // 根据导航组件的速度来控制动画播放
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        // 如果到达了主塔位置 就攻击主塔
        // if(Vector3.Distance(transform.position, MainTowerObject.Instance.transform.position) < 5.0f)
        // 这种写法 直接计算两点间的欧氏直线距离忽略导航网格与障碍 适合只关心直线距离的逻辑（比如触发范围判定、近身检测），但可能在有障碍时误判“到达”

        if (
            // pathPending == true 表示 NavMeshAgent 还在计算路径（路径尚未就绪），所以需要等到 false 才可信
            !agent.pathPending

            // hasPath == true 表示当前 agent 有一条路径（即 agent.path 不为 null/空）
            && agent.hasPath

            // pathStatus == PathComplete 表示路径计算成功并且目标点在可达的导航网格上
            // 其它可能值：PathInvalid（无效路径），PathPartial（部分路径，可能目标不可达）
            && agent.pathStatus == NavMeshPathStatus.PathComplete

            // remainingDistance 是沿着当前路径从当前位置到目标点的剩余路程（基于路径的长度，不是直线距离）
            // stoppingDistance 是你允许的到达容差半径。加上一个小容差 0.1f 用于避免浮点抖动导致误判
            && agent.remainingDistance <= agent.stoppingDistance + 0.1f && Time.time - atkTimer >= enemyInfo.atkOffset
           )
        {
            // 到达目的地
            animator.SetTrigger("Atk");
            atkTimer = Time.time;
        }
    }

    public void AtkEvent()
    {
        if (isDead)
            return;
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1.0f, LayerMask.GetMask("MainTower"));
        GameDataMgr.Instance.PlaySound("enemyAtk");
        foreach (var col in colliders)
        {
            if (col.TryGetComponent<MainTowerObject>(out var mainTower))
            {
                // 攻击主塔
                MainTowerObject.Instance.BeDamage(enemyInfo.atk);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    // 玩家动画组件
    private Animator animator;
    // 玩家攻击力
    private int atk;
    // 鼠标左右移动控制玩家旋转的速度
    private float roundSpeed = 70;
    public int Gold;
    public Transform FirePoint;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 移动动作的变换
        // 由于动作有位移 我们也应用了动作的位移 所以只要改变这两个值就会有动作的变化和速度的变化
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        // 角色旋转
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 0);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Roll");
        }
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }

    }

    /// <summary>
    /// 提供给外部 初始化玩家信息的方法 理论上atk之类的数据应该从配置表里获取 但是demo懒得改了先简化处理 直接写死了
    /// </summary>
    /// <param name="atk">角色攻击力</param>
    /// <param name="Gold">本场游戏初始金币数量</param>
    public void InitPlayerInfo(int atk, int Gold)
    {
        this.atk = atk;
        this.Gold = Gold;
        UpdateGold(); // 初始化时更新一次金币显示
    }
    
    /// <summary>
    /// 专门用于处理刀类武器攻击伤害检测的事件
    /// </summary>
    public void KnifeEvent()
    {
        // 进行范围伤害检测
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1.0f, LayerMask.GetMask("Enemy"));
        // 播放音效
        GameDataMgr.Instance.PlaySound("dagger");
        // 遍历检测到的碰撞体
        for(int i = 0; i < colliders.Length; i++)
        {
            // 如果怪物存活，得到碰撞到的对象身上的EnemyObject脚本，调用它的受伤方法
            if (colliders[i].TryGetComponent<EnemyObject>(out var enemy) && !enemy.isDead)
            {
                enemy.TakeDamage(atk);
                break; // 刀只伤害第一个敌人就停止
            }
        }
    }

    /// <summary>
    /// 专门用于处理枪类武器攻击伤害检测的事件
    /// </summary>
    public void ShootEvent()
    {
        // 进行射线伤害检测
        RaycastHit[] hits = Physics.RaycastAll(new Ray(FirePoint.position, transform.forward), 1000, LayerMask.GetMask("Enemy"));
        // 播放音效
        GameDataMgr.Instance.PlaySound("fire");
        // 遍历检测到的碰撞体
        for(int i = 0; i < hits.Length; i++)
        {
            // 如果怪物存活，得到碰撞到的对象身上的EnemyObject脚本，调用它的受伤方法
            if (hits[i].collider.TryGetComponent<EnemyObject>(out var enemy) && !enemy.isDead)
            {
                // 打击特效创建
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.curChooseRole.hitEff),
                    hits[i].point, 
                    Quaternion.LookRotation(hits[i].normal));
                Destroy(effObj, 1.0f); // 1秒后销毁特效

                enemy.TakeDamage(atk);
                break; // 枪只打中第一个敌人就停止
            }
        }
    }

    public void UpdateGold()
    {
        // 更新UI显示的金币数量
        UIManager.Instance.GetPanel<GamePanel>().UpdateGold(Gold);
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateGold();
    }

}

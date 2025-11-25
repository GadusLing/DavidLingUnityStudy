using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 圣像(主塔) 脚本
/// </summary>
public class MainTowerObject : MonoBehaviour
{
    private int curHP;
    private int maxHP;
    private bool isDead;

    private static MainTowerObject _instance;
    public static MainTowerObject Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    public void UpdateHP(int hp, int maxHp)
    {
        curHP = hp;
        maxHP = maxHp;
        // 更新UI
        UIManager.Instance.GetPanel<GamePanel>().UpdateMainHP(hp, maxHp);
    }

    public void BeDamage(int damage)
    {
        if (isDead) return;

        curHP -= damage;
        if (curHP <= 0)
        {
            curHP = 0;
            isDead = true;
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.Gold / 2, false); // 失败 只给一半金币奖励
        }
        // 更新UI
        UpdateHP(curHP, maxHP);
    }

    void OnDestroy()
    {
        // 过场景时对象被销毁 就会调用置空 静态引用被清空 清空静态字段能让 Instance 返回 null，表示当前没有活跃实例
        // Unity 的对象有两部分：C++ 层的原生对象（native object）和 C# 的托管包装（managed wrapper）。
        // Destroy(obj) 会销毁原生对象，但托管包装仍存在一段时间。
        // Unity 重载了 UnityEngine.Object 的 == 运算符：当原生对象被销毁时，"obj == null" 会返回 true（表现为“假空”），
        // 但实际上托管引用仍非 null（System.Object.ReferenceEquals(obj, null) 返回 false）。
        // 如果静态字段还持有这个托管引用（例如 _instance 没清空），托管对象会被引用而不会被 GC 回收，可能导致误用已销毁对象或内存无法及时释放。
        // 因此在 OnDestroy 中把静态引用设为 null 很重要。
        _instance = null;
    }
}

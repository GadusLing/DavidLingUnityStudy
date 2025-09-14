using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObj : TankBaseObj
{
    private Transform targetPos;// 目标点

    public Transform[] randomPos; // 巡逻点

    public Transform lookAtTarget; // 一直要观察的目标

    public float AttackDistance = 50f; // 到了距离就攻击
    public float AttackTime = 1f; // 攻击间隔
    private float currentTime = 0f;

    public Transform shootPos; // 子弹发射位置
    public GameObject bulletObj; // 子弹预制体

    public Texture MaxHPTexture; // 血条满
    public Texture HPTexture; // 血条当前

    private Rect MaxHPRect; // 血条满的区域
    private Rect HPRect; // 血条当前的区域
    //这里是结构体不用new，类一定要new初始化，不然报错

    private float showTime; // 血条显示时间

    // === 血条距离缩放相关变量 ===
    [Header("血条距离缩放设置")]
    public float baseHPBarSize = 100f; // 基础血条大小（像素宽度）
    public float referenceDistance = 10f; // 参考距离（在这个距离下血条保持基础大小）
    [Range(0.1f, 5f)]
    public float minScale = 0.2f; // 最小缩放比例（防止血条过小看不见）
    [Range(0.1f, 5f)]
    public float maxScale = 2f; // 最大缩放比例（防止血条过大占屏幕）

    void Start()
    {
        RandomTargetPos();
    }

    void Update()
    {
        if (targetPos != null)
        {
            transform.LookAt(targetPos);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            Vector3.Distance(transform.position, targetPos.position);
            if(Vector3.Distance(transform.position, targetPos.position) < 0.05f)
            {
                RandomTargetPos();
            }
        }
        if(lookAtTarget != null)
        {
            Turret.LookAt(lookAtTarget);
            if(Vector3.Distance(transform.position, lookAtTarget.position) <= AttackDistance)
            {
                currentTime += Time.deltaTime;
                if(currentTime >= AttackTime)
                {
                    Attack();
                    currentTime = 0;
                }
            }
        }

    }

    private void RandomTargetPos()
    {
        if(randomPos.Length == 0) return;
        int index = Random.Range(0, randomPos.Length);
        targetPos = randomPos[index];
    }

    public override void Attack()
    {
        if(shootPos == null || bulletObj == null) return;
        GameObject obj = Instantiate(bulletObj, shootPos.position, shootPos.rotation);
        BulletObj bullet = obj.GetComponent<BulletObj>();
        bullet.SetOwner(this);
    }

    public override void Die()
    {
        base.Die();
        GamePanel.Instance.UpdateScore(10);

    }

    /// <summary>
    /// 敌人血条绘制 - 根据距离动态缩放
    /// 
    /// 实现思路：
    /// 1. 将3D世界坐标转换为2D屏幕坐标
    /// 2. 计算摄像机到敌人的距离
    /// 3. 根据距离计算缩放比例（距离越远，血条越小）
    /// 4. 应用缩放比例到血条的宽度、高度和位置
    /// 5. 限制缩放范围，确保血条不会过大或过小
    /// </summary>
    private void OnGUI()
    {
        // 只有在血条显示时间大于0时才绘制
        if(showTime > 0)
        {
            // 递减显示时间
            showTime -= Time.deltaTime;
            
            // === 步骤1：坐标转换 ===
            // 将敌人的3D世界坐标转换为2D屏幕坐标
            // transform.position + Vector3.up * 2 表示在敌人头顶上方2个单位的位置显示血条
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
            
            // === 步骤2：可见性检查 ===
            // 如果物体在摄像机后面（z < 0），则不显示血条
            // 这避免了当敌人在摄像机后方时血条还显示在屏幕上的问题
            if(screenPos.z < 0) return;
            
            // Unity的屏幕坐标系Y轴是从下往上的，但GUI坐标系Y轴是从上往下的
            // 所以需要转换Y坐标：Screen.height - screenPos.y
            screenPos.y = Screen.height - screenPos.y;
            
            // === 步骤3：距离计算和缩放比例 ===
            // 计算摄像机到敌人的实际距离
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            
            // 计算缩放比例：referenceDistance / distance
            // 当distance = referenceDistance时，scaleFactor = 1（血条保持基础大小）
            // 当distance > referenceDistance时，scaleFactor < 1（血条变小）
            // 当distance < referenceDistance时，scaleFactor > 1（血条变大）
            float scaleFactor = referenceDistance / distance;
            
            // === 步骤4：缩放范围限制 ===
            // 使用Clamp限制缩放比例在合理范围内
            // 防止距离过近时血条过大，距离过远时血条过小看不见
            scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);
            
            // === 步骤5：应用缩放到血条尺寸 ===
            // 根据缩放比例计算实际的血条宽度和高度
            float barWidth = baseHPBarSize * scaleFactor;
            float barHeight = 15f * scaleFactor; // 基础高度15像素
            
            // === 步骤6：绘制背景血条（最大血量） ===
            // 设置血条背景的位置和大小
            MaxHPRect.x = screenPos.x - barWidth / 2; // 居中对齐
            MaxHPRect.y = screenPos.y + 50 * scaleFactor; // 在敌人头顶上方一定距离
            MaxHPRect.width = barWidth; // 完整宽度
            MaxHPRect.height = barHeight;
            GUI.DrawTexture(MaxHPRect, MaxHPTexture);

            // === 步骤7：绘制当前血条 ===
            // 设置当前血量血条的位置和大小
            HPRect.x = screenPos.x - barWidth / 2; // 与背景血条位置对齐
            HPRect.y = screenPos.y + 50 * scaleFactor;
            // 当前血条宽度 = (当前血量 / 最大血量) * 完整宽度
            // 这样血条长度会根据血量百分比动态变化
            HPRect.width = (float)HP / (float)maxHP * barWidth;
            HPRect.height = barHeight;
            GUI.DrawTexture(HPRect, HPTexture);
        }
    }

    public override void BeAttacked(TankBaseObj attacker)
    {
        base.BeAttacked(attacker);
        showTime = 4f; // 每次被攻击血条显示时间重置
    }
}

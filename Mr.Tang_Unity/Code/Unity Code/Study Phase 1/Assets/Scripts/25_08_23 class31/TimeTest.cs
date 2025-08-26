using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    /*
    * 问题1：在哪里可以设置物理更新的间隔时间？
    * 答案：可以在 Edit > Project Settings > Time 中设置 Fixed Timestep 的值来调整物理更新的间隔时间。
    * 默认情况下是0.02秒（即50次/秒）。
    */

    /*
    * 问题2：请问Time中的各个时间对于我们来说，可以用来做什么？至少选择两个时间变量来进行说明
    * 答案：Time类中的重要时间变量及其用途：
    * 
    * 1. Time.deltaTime
    *    - 上一帧到当前帧的时间间隔
    *    - 主要用于使物体运动或动画平滑，不受帧率影响
    *    - 示例：transform.Translate(Vector3.forward * speed * Time.deltaTime);
    * 
    * 2. Time.fixedDeltaTime
    *    - 物理引擎更新的固定时间间隔
    *    - 用于物理计算，确保物理模拟的稳定性
    *    - 在FixedUpdate中使用，处理刚体运动和碰撞检测
    *    
    * 3. Time.timeScale
    *    - 时间缩放系数，控制游戏时间流逝速度
    *    - 可用于实现游戏暂停(设为0)或慢动作效果(设为0-1之间)
    *    - 不影响fixedDeltaTime和实际时间
    */

    /*
    * 问题：为什么用了deltaTime就不受帧率影响？不用deltaTime帧率会如何影响物体和动画？
    * 
    * 答案：
    * 使用Time.deltaTime可以让物体的移动或动画与帧率无关，实现“帧率独立”。
    * 
    * 原理说明：
    * - Time.deltaTime表示上一帧到当前帧的时间间隔（秒）。
    * - 如果直接用速度移动物体（如：transform.Translate(Vector3.forward * speed);），
    *   那么每帧都会移动speed单位，帧率高时移动得快，帧率低时移动得慢，导致不同设备上表现不一致。
    * - 正确做法是：transform.Translate(Vector3.forward * speed * Time.deltaTime);
    *   这样每秒移动的距离是speed单位，不管帧率高低，移动速度都一致。
    * 
    * 举例说明：
    * - 假设speed=5，每秒60帧，deltaTime约为1/60=0.0167秒。
    *   每帧移动距离：5 * 0.0167 ≈ 0.0835单位，1秒移动5单位。
    * - 如果帧率只有30帧，deltaTime约为1/30=0.0333秒。
    *   每帧移动距离：5 * 0.0333 ≈ 0.1665单位，1秒同样移动5单位。
    * 
    * 总结：
    * - 不用deltaTime，物体和动画会因帧率不同而快慢不一，体验不一致。
    * - 用了deltaTime，移动和动画速度与帧率无关，保证所有设备表现一致。
    */

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

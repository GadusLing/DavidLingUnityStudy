using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开火点位置 左上 上 右上 左 右 左下 下 右下
/// </summary>
public enum E_FireType
{
    TopLeft,
    Top,
    TopRight,
    Left,
    Right,
    BottomLeft,
    Bottom,
    BottomRight
}

public class FireObject : MonoBehaviour
{
    public E_FireType fireType;
    private FireInfo fireInfo; // 当前开火点信息
    private int curNum; // 当前发射的子弹数量
    private float curCd; // 当前子弹间隔时间
    private float curDelay; // 当前子弹组间隔时间
    private BulletInfo curBulletInfo; // 当前组开火点要发射的子弹信息

    private float changeAngle; // 散射时每颗子弹的散射角度
    private Vector3 screenPos; // 物体在屏幕上的位置
    private Vector3 initDir; // 初始方向，主要用于根据子弹数量计算散射角度
    private Vector3 curDir; // 当前方向，主要用于记录上一次的方向，方便计算散射角度

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
        ResetFireInfo();
        UpdateFire();
    }

    // 根据开火点的位置类型来更新位置
    private void UpdatePos()
    {
        screenPos.z = 280; // 根据主玩家与摄像机的距离设置z轴坐标，确保子弹和飞机在同一平面上
        switch (fireType)
        {
            case E_FireType.TopLeft:
                screenPos.x = 0;
                screenPos.y = Screen.height;
                initDir = Vector3.right;
                break;
            case E_FireType.Top:
                screenPos.x = Screen.width / 2;
                screenPos.y = Screen.height;
                initDir = Vector3.right;
                break;
            case E_FireType.TopRight:
                screenPos.x = Screen.width;
                screenPos.y = Screen.height;
                initDir = Vector3.left;
                break;
            case E_FireType.Left:
                screenPos.x = 0;
                screenPos.y = Screen.height / 2;
                initDir = Vector3.right;
                break;
            case E_FireType.Right:
                screenPos.x = Screen.width;
                screenPos.y = Screen.height / 2;
                initDir = Vector3.left;
                break;
            case E_FireType.BottomLeft:
                screenPos.x = 0;
                screenPos.y = 0;
                initDir = Vector3.right;
                break;
            case E_FireType.Bottom:
                screenPos.x = Screen.width / 2;
                screenPos.y = 0;
                initDir = Vector3.right;
                break;
            case E_FireType.BottomRight:
                screenPos.x = Screen.width;
                screenPos.y = 0;
                initDir = Vector3.left;
                break;
        }
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);// 将屏幕坐标转换为世界坐标
    }

    // 重置当前要发射的开火点数据
    private void ResetFireInfo()
    {
        if(curCd != 0 &&curNum != 0)// 当cd和num不为0时，说明开火点正在工作，不用重置
            return;
        
        if(fireInfo != null)
        {
            curDelay -= Time.deltaTime;
            if(curDelay > 0) return; // 还没有到达每组子弹的delay时间，继续等待
        }

        if(fireInfo != null && curNum == 0) // 一轮发射结束且等待完成
        {
            fireInfo = null;
            curDelay = 0f;
            curCd = 0f;
        }

        // 从数据管理器中随机获取一条开火点数据
        List<FireInfo> list = GameDataMgr.Instance.fireData.fireList;
        if (list != null && list.Count > 0)
        {
            fireInfo = list[Random.Range(0, list.Count)];
            // 我们不能直接改变fireInfo的数据，因为它是数据管理器中的数据，所以用临时变量来存储
            curNum = fireInfo.num;
            curCd = fireInfo.cd;
            curDelay = fireInfo.delay;

            // 通过开火点数据 取出当前要使用的子弹信息
            string[] arr = fireInfo.ids.Split(',');
            int id = Random.Range(int.Parse(arr[0]), int.Parse(arr[1]) + 1);
            curBulletInfo = GameDataMgr.Instance.bulletData.bulletList[id - 1];

            if (fireInfo.type == 2) // 散射
            {
                switch(fireType)
                {
                    case E_FireType.TopLeft:
                    case E_FireType.TopRight:
                    case E_FireType.BottomLeft:
                    case E_FireType.BottomRight:
                        changeAngle = 90f / (curNum + 1); // 角上的开火点散射总角度为90度

                        break;
                    case E_FireType.Top:
                    case E_FireType.Left:
                    case E_FireType.Right:
                    case E_FireType.Bottom:
                        changeAngle = 180f / (curNum + 1); // 边上的开火点散射总角度为180度
                        break;
                }
            }
        }
    }

    private void UpdateFire()
    {
        if(curCd == 0 && curNum == 0) return; // 当前没有子弹需要发射
        curCd -= Time.deltaTime;
        if(curCd > 0) return; // 还没有到达每颗子弹的cd时间，继续等待
        GameObject bullet;
        BulletObject bulletObj;
        switch(fireInfo.type)
        {
            case 1: // 顺序
                
                bullet = Instantiate(Resources.Load<GameObject>(curBulletInfo.resName), transform.position, Quaternion.LookRotation(
                    PlayerObject.Instance.transform.position - transform.position));
                bulletObj = bullet.AddComponent<BulletObject>();
                bulletObj.InitInfo(curBulletInfo);
                --curNum;
                curCd = curNum == 0 ? 0 : fireInfo.cd; // 当当前子弹数量为0时，说明当前组子弹发射完毕，重置cd为0，等待delay时间后重置
                break;
            case 2: // 散射
                if(curCd == 0)// 无CD代表一瞬间发射所有散弹
                {
                    for(int i = 0; i < curNum; i++)
                    {
                        curDir = Quaternion.AngleAxis(changeAngle * i, Vector3.up) * initDir; // 计算散射方向
                        bullet = Instantiate(Resources.Load<GameObject>(curBulletInfo.resName), transform.position, Quaternion.LookRotation(curDir));
                        bulletObj = bullet.AddComponent<BulletObject>();
                        bulletObj.InitInfo(curBulletInfo);
                    }
                    curCd = curNum = 0; // 瞬间发完所有散弹 所以cd和num都重置为0
                }
                else
                {
                    curDir = Quaternion.AngleAxis(changeAngle * (fireInfo.num - curNum), Vector3.up) * initDir; // 计算散射方向
                    bullet = Instantiate(Resources.Load<GameObject>(curBulletInfo.resName), transform.position, Quaternion.LookRotation(curDir));
                    bulletObj = bullet.AddComponent<BulletObject>();
                    bulletObj.InitInfo(curBulletInfo);
                    --curNum;
                    curCd = curNum == 0 ? 0 : fireInfo.cd; // 当当前子弹数量为0时，说明当前组子弹发射完毕，重置cd为0，等待delay时间后重置
                }
                break;
        }
    }
}

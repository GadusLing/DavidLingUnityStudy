using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private static PlayerObject instance;
    public static PlayerObject Instance => instance;
    public int CurrentHP; // 当前生命值
    public int MaxHP; // 最大生命值
    public float speed; // 移动速度
    public float rotationSpeed; // 旋转速度 忘写了，底下配个40吧
    public bool isDead = false; // 是否死亡 
    private Quaternion targetRotation; // 目标旋转四元数角度

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 根据选择的角色初始化血量
        RoleInfo roleInfo = GameDataMgr.Instance.GetCurrentRoleInfo();
        MaxHP = roleInfo.HP;
        CurrentHP = MaxHP;
        
        // 更新UI显示
        GamePanel.Instance.ChangeHP(CurrentHP);
    }

    public void Dead()
    {
        isDead = true;
        GGPanel.Instance.ShowMe();
    }

    public void Wound()
    {
        if( isDead ) return;
        CurrentHP -= 1;
        if (CurrentHP < 0) CurrentHP = 0;
        GamePanel.Instance.ChangeHP(CurrentHP);
        if (CurrentHP == 0)
        {
            Dead();
        }
    }

    private float hValue;
    private float vValue;
    private Vector3 currentPos;// 当前要去到的位置
    private Vector3 frontPos; // 位移前位置
    void Update()
    {
        if( isDead ) return;
        hValue = Input.GetAxisRaw("Horizontal");
        vValue = Input.GetAxisRaw("Vertical");
        if(hValue == 0)
        {
            targetRotation = Quaternion.identity;
        }
        else
        {
            targetRotation = hValue < 0 ? Quaternion.AngleAxis(40, Vector3.forward) : Quaternion.AngleAxis(-40, Vector3.forward);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        // 记录位移前位置
        frontPos = transform.position;

        // 移动
        transform.Translate(Vector3.forward * vValue * speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.right * hValue * speed * Time.deltaTime, Space.World);

        // 判断当前位置是否超出边界，将飞船坐标系转化成屏幕坐标系，然后通过分辨率计算边界
        currentPos = Camera.main.WorldToScreenPoint(transform.position);
        if(currentPos.x <= 0 || currentPos.x >= Screen.width)
        {
            // 超出边界，返回到位移前X位置
            transform.position = new Vector3(frontPos.x, transform.position.y, transform.position.z);
        }
        if(currentPos.y <= 0 || currentPos.y >= Screen.height)
        {
            // 超出边界，返回到位移前Y位置
            transform.position = new Vector3(transform.position.x, transform.position.y, frontPos.z);
        }
        // 这样分开写避免在比如左边界，但是按WS上下无法移动的问题，每次只回溯一个轴向

        // 射线检测 鼠标点击可以消除子弹
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000, LayerMask.GetMask("Bullet")))
            {
                hitInfo.collider.GetComponent<BulletObject>().Dead();
            }
        }
    }
}

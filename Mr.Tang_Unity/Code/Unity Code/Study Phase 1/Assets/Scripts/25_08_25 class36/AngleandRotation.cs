using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleandRotation : MonoBehaviour
{
    // ��һ�⣺
    // ʹ����֮ǰ������̹��Ԥ���壬��̹�������һ�����������Դ������弴�ɣ�
    // �������ԭ����ת������һ��չ��̨

    // �ڶ��⣺
    // �ڵ�һ��Ļ����ϣ���̹�˵���̨�����Զ�����������ת���ڹܿ����Զ�����̧��

    // �����⣺
    // ����3�����壬ģ��̫������������֮�����ת�ƶ�

    public GameObject Tank;
    public GameObject TankBase;
    public float rotateSpeed = 30;
    public float turretRotateSpeed = 30;
    public float barrelRotateSpeed = 30;
    public Transform Turret; // ��̨
    public Transform Barrel; // �ڹ�

    // ��̨���������ת�Ƕȣ�����45�ȣ�
    public float turretMaxAngle = 45f;

    // �ڹ�����������Ǻ͸���
    public float barrelMaxUpAngle = -10f; // ��󸩽�
    public float barrelMaxDownAngle = 10f;   // �������


    // ��¼��ǰ��̨��ת����1Ϊ��ת��-1Ϊ��ת��
    private int turretDirection = 1;
    private float currentBarrelAngle = 0f; // ��¼��ǰ�ڹ�����
    private int barrelDirection = -1;      // -1Ϊ��̧��1Ϊ��ѹ

    void Start()
    {

    }

    void Update()
    {
        // �õ���Χ��Y����ת��̹�˻����һ��ת
        if (TankBase != null)
            TankBase.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // ��̨�Զ�����������ת
        AutoRotateTurret();

        // �ڹ��Զ�����̧��
        AutoRotateBarrel();
    }

    /// <summary>
    /// ��̨�Զ�����������ת��-turretMaxAngle ~ turretMaxAngle��
    /// </summary>
    void AutoRotateTurret()
    {
        if (Turret == null) return;

        // �Ը�����Ϊ�ο�����ȡ����ŷ����
        float y = Turret.localEulerAngles.y;
        // �����Ƕȵ�-180~180
        if (y > 180) y -= 360;

        // �ж��Ƿ񵽴����Ƕȣ��ı���ת����
        if (y >= turretMaxAngle && turretDirection > 0)
            turretDirection = -1;
        else if (y <= -turretMaxAngle && turretDirection < 0)
            turretDirection = 1;

        // ����ǰ������ת
        Turret.Rotate(Vector3.up * turretRotateSpeed * turretDirection * Time.deltaTime);
    }

    /// <summary>
    /// �ڹ��Զ�����̧��barrelMaxDownAngle ~ barrelMaxUpAngle��
    /// </summary>
    void AutoRotateBarrel()
    {
        if (Barrel == null) return;

        // ÿ֡�ۼ�����
        currentBarrelAngle += barrelRotateSpeed * barrelDirection * Time.deltaTime;

        // �������Ƿ�Χ
        currentBarrelAngle = Mathf.Clamp(currentBarrelAngle, barrelMaxDownAngle, barrelMaxUpAngle);

        // �ж��Ƿ񵽴���λ���ı䷽��
        if (currentBarrelAngle <= barrelMaxDownAngle && barrelDirection == -1)
            barrelDirection = 1;
        else if (currentBarrelAngle >= barrelMaxUpAngle && barrelDirection == 1)
            barrelDirection = -1;

        // �����ڹܱ�����ת��ֻ��x�ᣩ
        Barrel.localRotation = Quaternion.Euler(currentBarrelAngle, 0, 0);
    }
}


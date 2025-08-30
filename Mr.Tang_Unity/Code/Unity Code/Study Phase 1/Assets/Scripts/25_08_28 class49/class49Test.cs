using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class class49Test : MonoBehaviour
{
    public GameObject tank;

    private GameObject tankInstance;
    private GameObject Tower;
    private GameObject Canon;
    private GameObject[] wheels;   // �ĸ�����
    private Camera cam;            // �Լ����������
    private Camera camS;            

    // ̹���ƶ�����
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    // ��̨��ת����
    public float turretRotationSpeed = 1f;

    // �ڹ�̧������
    public float cannonElevationSpeed = 10f;

    // ����������
    public float cameraFollowSpeed = 5f;
    private Vector3 cameraOffset = new Vector3(0, 5, -9);

    void Start()
    {
        // ʵ����̹��
        tankInstance = Instantiate(tank, Vector3.zero, Quaternion.identity);

        // ����������
        Tower = FindChild(tankInstance, "TankFree_Tower");
        Canon = FindChild(Tower, "TankFree_Canon");

        wheels = new GameObject[]
        {
            FindChild(tankInstance, "TankFree_Wheel_b_left"),
            FindChild(tankInstance, "TankFree_Wheel_b_right"),
            FindChild(tankInstance, "TankFree_Wheel_f_left"),
            FindChild(tankInstance, "TankFree_Wheel_f_right"),
        };

        // ����һ̨�����
        GameObject camObj = new GameObject("TankCamera");
        cam = camObj.AddComponent<Camera>();
        // �����ڹܼ粿��Ӱ��
        GameObject camSObj = new GameObject("TankSCamera");
        camS = camSObj.AddComponent<Camera>();

        // ���ڹܼ粿��Ӱ������Ϊ̹�˵�������
        cam.transform.SetParent(tankInstance.transform);
        camS.transform.SetParent(tankInstance.transform);

        // ���÷���Ч�� - ���ҷ���
        // ��������x=0, y=0, width=0.5, height=1
        cam.rect = new Rect(0, 0, 0.5f, 1f);
        // �Ҳ������x=0.5, y=0, width=0.5, height=1
        camS.rect = new Rect(0.5f, 0, 0.5f, 1f);

        // ��ʼλ��
        cam.transform.position = tankInstance.transform.position + new Vector3(0, 5, -9);
        camS.transform.position = tankInstance.transform.position + new Vector3(0.92f, 0.77f, -0.66f);
        cam.transform.LookAt(tankInstance.transform);
    }

    void Update()
    {
        if (tankInstance == null || cam == null) return;

        // ===== ̹���ƶ� =====
        float moveInput = Input.GetAxis("Vertical");   // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D
        tankInstance.transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        tankInstance.transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // ===== ������ת =====
        foreach (var wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right, moveInput * moveSpeed * 50 * Time.deltaTime);
        }

        // ===== ��̨��ת =====
        float mouseX = Input.GetAxis("Mouse X");
        Tower?.transform.Rotate(Vector3.up, mouseX * turretRotationSpeed);

        // ===== �ڹ�̧����� =====
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Canon?.transform.Rotate(Vector3.left, scrollInput * cannonElevationSpeed);

        // ===== ������� =====
        if (Input.GetMouseButton(1)) // �Ҽ���ס
        {
            float mX = Input.GetAxis("Mouse X");
            float mY = Input.GetAxis("Mouse Y");

            // ˮƽ��ת
            cam.transform.RotateAround(tankInstance.transform.position, Vector3.up, mX * 1000f * Time.deltaTime);

            // ��ֱ��ת
            cam.transform.RotateAround(tankInstance.transform.position, cam.transform.right, -mY * 1000f * Time.deltaTime);
        }

        // ʼ�����������̹��
        cam.transform.LookAt(tankInstance.transform);
    }

    /// <summary>
    /// �Ӹ���������������壬����Ҳ�������
    /// </summary>
    private GameObject FindChild(GameObject parent, string name)
    {
        var child = parent.transform.Find(name);
        if (child == null)
        {
            Debug.LogError($"δ�ҵ���Ϊ '{name}' �������壡");
            return null;
        }
        return child.gameObject;
    }
}

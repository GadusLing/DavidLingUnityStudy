using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    //1.ʹ��֮ǰ��̹��Ԥ���壬��WASD������̹�˵�ǰ�����ˣ�����ת��
    //2.����һ��Ļ����ϣ���������ƶ�������̨��ת��
    //�������Ĵ��뱣���ã�֮�������õ�
    public GameObject tank;

    private GameObject tankInstance;
    private GameObject Tower;

    // ̹���ƶ�����
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    // ��̨��ת����
    public float turretRotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // ʵ����̹�˲���������
        tankInstance = Instantiate(tank, new Vector3(0, 0, 0), Quaternion.identity);

        // ������̨����
        Tower = tankInstance.transform.Find("TankFree_Tower").gameObject;
        if (Tower == null)
        {
            Debug.LogError("δ�ҵ���Ϊ'TankFree_Tower'����̨����");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tankInstance == null) return;

        // ��ȡ��������
        float moveInput = Input.GetAxis("Vertical");   // W/S��
        float turnInput = Input.GetAxis("Horizontal"); // A/D��

        // �ƶ�̹��
        tankInstance.transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        tankInstance.transform.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);

        // ��ȡ������벢��ת��̨
        if (Tower != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            Tower.transform.Rotate(Vector3.up, mouseX * turretRotationSpeed);
        }
    }
}
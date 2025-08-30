using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomLookAt : MonoBehaviour
{
    // ʹ��֮ǰ��̹��Ԥ���壬����������Ը������ƶ�������һֱ����̹��
    public GameObject Tank;
    private GameObject newTank;
    private Camera followCamera;

    // Start is called before the first frame update
    void Start()
    {
        // ʵ����̹��
        newTank = Instantiate(Tank, new Vector3(0, 0, 0), Quaternion.identity);

        // ����һ���µ����������
        GameObject camObj = new GameObject("FollowCamera");
        followCamera = camObj.AddComponent<Camera>();

        // ���������Ϊ̹�˵��Ӷ���
        camObj.transform.SetParent(newTank.transform);

        // �����������ʼλ�ã��ɸ�����Ҫ�������λ�ã�
        camObj.transform.localPosition = new Vector3(0, 2, -6);
        camObj.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        // �������ʼ�տ���̹��
        if (followCamera != null && newTank != null)
        {
            followCamera.transform.LookAt(newTank.transform);
        }
    }
}

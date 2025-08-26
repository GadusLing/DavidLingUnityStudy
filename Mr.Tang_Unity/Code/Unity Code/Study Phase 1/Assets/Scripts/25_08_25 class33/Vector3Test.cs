using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Test : MonoBehaviour
{
    // ��Ŀ1��
    // һ���ն����Ϲ���һ���ű�������ű���������Ϸ����ʱ��
    // �ڳ����д�����һ�������Cube���ɵĽ���������ʾ��ʵ����Ԥ�������ʵ�����Դ������巽����

    // ��Ŀ2��
    // ��������д������Щ���ö����Լ����泯���ƶ���Ϊʲô�������Ի�ͼ˵����
    // this.transform.Translate(Vector3.forward, Space.World);
    // this.transform.Translate(Vector3.forward, Space.Self);
    // this.transform.Translate(this.transform.forward, Space.Self);
    // this.transform.Translate(this.transform.forward, Space.World);

    // ��Ŀ3��
    // ʹ����֮ǰ������������Ԥ���壬������Գ��Լ����泯����ǰ�ƶ�

    public GameObject prefab;
    public GameObject Tank;

    void Start()
    {
        // ��vector3 zero 000 ԭ�㴦���������� ����ת
        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);


        Tank = Instantiate(Tank, new Vector3(5, 5, 5), Quaternion.identity);
    }

    void Update()
    {
        Tank.transform.Translate(Tank.transform.forward * Time.deltaTime * 1, Space.World);
    }
}

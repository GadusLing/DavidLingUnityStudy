using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateTransformation : MonoBehaviour
{
    /// <summary>
    /// ��Ŀ1��������Aǰ������һ��������
    /// Ҫ��
    /// 1. ����������A��λ��
    /// 2. ������A��ǰ������(-1,0,1)������һ��������
    /// </summary>
    [ContextMenu("��ǰ����������")]
    private void CreateObjectInFront()
    {
        //Vector3 pos = transform.TransformPoint(new Vector3(-1, 0, 1));
        GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObj.name = "��ǰ������";
        newObj.transform.position = transform.TransformPoint(new Vector3(-1, 0, 1));
    }

    /// <summary>
    /// ��Ŀ2��������Aǰ��������������
    /// Ҫ��
    /// 1. ����������A��λ��
    /// 2. ������A��ǰ������3������
    /// 3. ������������λ�÷ֱ�Ϊ��
    ///    - (0,0,1)
    ///    - (0,0,2)
    ///    - (0,0,3)
    /// </summary>
    [ContextMenu("ǰ������������")]
    private void CreateThreeSpheresInFront()
    {
        // TODO: ������ʵ����Ŀ2���߼�
        for (int i = 1; i <= 3; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "����" + i;
            sphere.transform.position = transform.TransformPoint(new Vector3(0, 0, i));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateObjectInFront();
        CreateThreeSpheresInFront();
    }

}

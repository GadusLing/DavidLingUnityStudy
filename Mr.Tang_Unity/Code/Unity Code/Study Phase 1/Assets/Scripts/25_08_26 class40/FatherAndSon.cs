using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherAndSon : MonoBehaviour
{
    //��ΪTransformдһ����չ���������Խ������Ӷ������ֵĳ��̽�������ı����ǵ�˳��
    //���̵ֶ���ǰ�棬���ֳ����ں���

    //��ΪTransformдһ����չ����������һ�����ֲ����Ӷ��󣬼�ʹ���Ӷ�����Ӷ���Ҳ�ܲ��ҵ�

    // Start is called before the first frame update
    void Start()
    {
        transform.SortChildrenByName();

        Transform target = transform.DepthFindChildren("children2");
        if (target != null)
        {
            print("�ҵ������壺" + target.name);
        }
        else
        {
            print("û���ҵ�ָ�����ֵ�������");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

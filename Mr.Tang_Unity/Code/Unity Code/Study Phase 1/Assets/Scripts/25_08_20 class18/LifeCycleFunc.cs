using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Unity�������ں�����Ŀ��
 * 1. ����ֽ��Ĭд�����ĸ����������ں���
 *    ��д�����Ƿֱ��ʱִ��
 *    �Լ�ִ�е��Ⱥ�˳����ʲô
 * 
 * 2. ����������˵�����������ں���������ĳ���еĳ�Ա��Ϊʲô
 *    Unity�����Զ���ִ����Щ����ĺ�����
 */

public class LifeCycleFunc : MonoBehaviour
{
    // 1. Awake - ���󱻴���ʱ�������ã���Start֮ǰ
    void Awake()
    {
        Debug.Log("1. Awake - ���󴴽�ʱ����");
    }

    // 2. OnEnable - ���󼤻�ʱ���ã�ÿ�μ�������
    void OnEnable()
    {
        Debug.Log("2. OnEnable - ���󼤻�ʱ����");
    }

    // 3. Start - ��һ֡����֮ǰ���ã���Awake֮��
    void Start()
    {
        Debug.Log("3. Start - ��һ֡����ǰ����");
    }

    // 4. FixedUpdate - �̶�ʱ�������ã������������
    void FixedUpdate()
    {
        Debug.Log("4. FixedUpdate - �̶�ʱ��������");
    }

    // 5. Update - ÿ֡����һ��
    void Update()
    {
        Debug.Log("5. Update - ÿ֡����");
    }

    // 6. LateUpdate - ������Update������ɺ����
    void LateUpdate()
    {
        Debug.Log("6. LateUpdate - ����Update�����");
    }

    // 7. OnDisable - ����ʧ��ʱ����
    void OnDisable()
    {
        Debug.Log("7. OnDisable - ����ʧ��ʱ����");
    }

    // 8. OnDestroy - ��������ʱ����
    void OnDestroy()
    {
        Debug.Log("8. OnDestroy - ��������ʱ����");
    }

    /*
     * ����ִ��˳��
     * Awake �� OnEnable �� Start �� FixedUpdate/Update/LateUpdate(ѭ��) �� OnDisable �� OnDestroy
     * 
     * �ر�˵����
     * - OnEnable��ÿ��GameObject��Component������ʱ����
     * - OnDisable��ÿ��GameObject��Component��ʧ��ʱ����
     * - ������󱻶�μ���/ʧ�OnEnable��OnDisable�ᱻ��ε���
     * - Startֻ�ڶ������������е���һ��
     * 
     * ΪʲôUnity���Զ�ִ����Щ������
     * ��Unityʹ�÷�����ƣ�������ʱ���̳���MonoBehaviour������
     * �Ƿ������Щ�ض����Ƶķ�����������ھ��Զ����á�
     * ��Щ��������Ҫoverride�ؼ��֣�Unityͨ��������ʶ�𲢵��á�
     */
}
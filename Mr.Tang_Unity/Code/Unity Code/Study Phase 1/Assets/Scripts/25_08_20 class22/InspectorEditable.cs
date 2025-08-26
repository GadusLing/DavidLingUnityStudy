using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Inspector���༭��Ŀ��
 * 1. ����ù�����Ա����Inspector���������
 *    �����˽�л򱣻���Ա������Inspector���������
 * 
 * 2. Ϊʲô�Ӳ�ͬ�����ԣ���Inspector�����ϻ��в�ͬ��Ч��
 *    ��˵��������
 */

public class InspectorEditable : MonoBehaviour
{
    // ��Ŀ1���

    // 1.1 �ù�����Ա����Inspector�������ʾ - ʹ�� [HideInInspector]
    [HideInInspector]
    public int publicButHidden = 100;

    // ��ͨ������Ա������Inspector����ʾ��
    public string normalPublicField = "�����ʾ��Inspector��";

    // 1.2 ��˽�г�Ա��Inspector�������ʾ - ʹ�� [SerializeField]
    [SerializeField]
    private int privateButVisible = 50;

    [SerializeField]
    private float privateFloat = 3.14f;

    // ��ͨ˽�г�Ա��������Inspector����ʾ��
    private string normalPrivateField = "�ⲻ����ʾ��Inspector��";

    // ������Աͬ������ʹ�� [SerializeField] ��ʾ
    [SerializeField]
    protected bool protectedButVisible = true;

    // ��Ŀ2��� - ��ͬ���Ե�Ч����

    // [Range] - ����������
    [Range(0, 100)]
    public int sliderValue = 50;

    // [Header] - �����������
    [Header("�������")]
    public int playerHealth = 100;
    public float playerSpeed = 5.0f;

    // [Space] - ��ӿհ׼��
    [Space(10)]
    public string playerName = "Player";

    // [Tooltip] - �����ʾ��Ϣ
    [Tooltip("������ҵľ���ֵ")]
    public int experience = 0;

    // [TextArea] - �����ı������
    [TextArea(3, 5)]
    public string description = "����������Ϣ...";

    // [Multiline] - �򵥶����ı�
    [Multiline]
    public string simpleMultiline = "�򵥶����ı�";

    void Start()
    {
        // ��ʾ������Щ�ֶ�
        Debug.Log($"���صĹ����ֶ�ֵ: {publicButHidden}");
        Debug.Log($"�ɼ���˽���ֶ�ֵ: {privateButVisible}");
        Debug.Log($"��ͨ˽���ֶ�ֵ: {normalPrivateField}");
    }

    void Update()
    {
        
    }

    /*
     * ���ܽ᣺
     * 
     * 1. ����Inspector��ʾ��
     *    - [HideInInspector]: ���ع�����Ա
     *    - [SerializeField]: ��ʾ˽��/������Ա
     * 
     * 2. ���Բ�����ͬЧ����ԭ��
     *    Unity��Inspectorϵͳͨ��������ƶ�ȡ��Щ����(Attribute)��
     *    Ȼ����ݲ�ͬ��������������Ⱦ��ͬ��UI�ؼ���
     *    
     *    - [Range]: ��ȾΪ�������ؼ�
     *    - [Header]: ��ȾΪ�����ı�
     *    - [Space]: ��Ӵ�ֱ���
     *    - [Tooltip]: ��������ͣ��ʾ
     *    - [TextArea]: ��ȾΪ�ɵ�����С���ı�����
     *    - [Multiline]: ��ȾΪ�̶��Ķ����ı���
     *    
     *    ��Щ����ʵ������Ԫ���ݣ�Unity�༭���ڹ���Inspector����ʱ
     *    ���ȡ��ЩԪ���ݲ���Ӧ�ص���UI��ʾЧ����
     */
}

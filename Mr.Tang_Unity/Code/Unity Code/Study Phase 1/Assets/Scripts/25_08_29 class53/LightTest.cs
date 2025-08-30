using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    /*
     * ��ϰ��Ŀ1��ͨ�������ϵ��Դģ��һ������Ĺ�ԴЧ��
     * - �������Դ���
     * - ���ù�Դ��ɫΪů��ɫ��ģ��������棩
     * - ������Դǿ�Ⱥͷ�Χ
     * - ��ѡ����ӹ�Դ��˸Ч��ģ������ҡҷ
     */

    /*
     * ��ϰ��Ŀ2��ͨ�������Ϸ����ģ���������ȵı仯
     * - ��������������ģ��̫���⣩
     * - ���ù�Դ����ͽǶ�
     * - ͨ��ʱ��仯������Դǿ�Ⱥ���ɫ
     * - ģ����ճ�������Ĺ��ձ仯����
     * - ���Խ����պ�ʵ�ָ���ʵ����ҹѭ��Ч��
     */

    public Light light;
    public float moveSpeed = 0.5f;// �ƶ��ٶ�
    public float intensitySpeed = 0.5f;// �����ǿ�仯�ٶ�
    public float minIntensity = 0.8f;// ��С��ǿ
    public float maxIntensity = 1.0f;// ����ǿ
    public Transform lightTransform;
    public float rotateSpeed = 10f;// ��ת�ٶ�

    // Update is called once per frame
    void Update()
    {
        light.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if(light.transform.position.x >= 0.45)
        {
            moveSpeed = -moveSpeed;
        }
        else if(light.transform.position.x < 0.3)
        {
            moveSpeed = -moveSpeed;
        }

        light.intensity += intensitySpeed * Time.deltaTime;
        if(light.intensity >= maxIntensity)
        {
            intensitySpeed = -intensitySpeed;
        }
        else if(light.intensity <= minIntensity)
        {
            intensitySpeed = -intensitySpeed;
        }

        lightTransform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
}

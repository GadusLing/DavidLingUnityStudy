using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    /*
    * ����1���������������������µļ��ʱ�䣿
    * �𰸣������� Edit > Project Settings > Time ������ Fixed Timestep ��ֵ������������µļ��ʱ�䡣
    * Ĭ���������0.02�루��50��/�룩��
    */

    /*
    * ����2������Time�еĸ���ʱ�����������˵������������ʲô������ѡ������ʱ�����������˵��
    * �𰸣�Time���е���Ҫʱ�����������;��
    * 
    * 1. Time.deltaTime
    *    - ��һ֡����ǰ֡��ʱ����
    *    - ��Ҫ����ʹ�����˶��򶯻�ƽ��������֡��Ӱ��
    *    - ʾ����transform.Translate(Vector3.forward * speed * Time.deltaTime);
    * 
    * 2. Time.fixedDeltaTime
    *    - ����������µĹ̶�ʱ����
    *    - ����������㣬ȷ������ģ����ȶ���
    *    - ��FixedUpdate��ʹ�ã���������˶�����ײ���
    *    
    * 3. Time.timeScale
    *    - ʱ������ϵ����������Ϸʱ�������ٶ�
    *    - ������ʵ����Ϸ��ͣ(��Ϊ0)��������Ч��(��Ϊ0-1֮��)
    *    - ��Ӱ��fixedDeltaTime��ʵ��ʱ��
    */

    /*
    * ���⣺Ϊʲô����deltaTime�Ͳ���֡��Ӱ�죿����deltaTime֡�ʻ����Ӱ������Ͷ�����
    * 
    * �𰸣�
    * ʹ��Time.deltaTime������������ƶ��򶯻���֡���޹أ�ʵ�֡�֡�ʶ�������
    * 
    * ԭ��˵����
    * - Time.deltaTime��ʾ��һ֡����ǰ֡��ʱ�������룩��
    * - ���ֱ�����ٶ��ƶ����壨�磺transform.Translate(Vector3.forward * speed);����
    *   ��ôÿ֡�����ƶ�speed��λ��֡�ʸ�ʱ�ƶ��ÿ죬֡�ʵ�ʱ�ƶ����������²�ͬ�豸�ϱ��ֲ�һ�¡�
    * - ��ȷ�����ǣ�transform.Translate(Vector3.forward * speed * Time.deltaTime);
    *   ����ÿ���ƶ��ľ�����speed��λ������֡�ʸߵͣ��ƶ��ٶȶ�һ�¡�
    * 
    * ����˵����
    * - ����speed=5��ÿ��60֡��deltaTimeԼΪ1/60=0.0167�롣
    *   ÿ֡�ƶ����룺5 * 0.0167 �� 0.0835��λ��1���ƶ�5��λ��
    * - ���֡��ֻ��30֡��deltaTimeԼΪ1/30=0.0333�롣
    *   ÿ֡�ƶ����룺5 * 0.0333 �� 0.1665��λ��1��ͬ���ƶ�5��λ��
    * 
    * �ܽ᣺
    * - ����deltaTime������Ͷ�������֡�ʲ�ͬ��������һ�����鲻һ�¡�
    * - ����deltaTime���ƶ��Ͷ����ٶ���֡���޹أ���֤�����豸����һ�¡�
    */

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

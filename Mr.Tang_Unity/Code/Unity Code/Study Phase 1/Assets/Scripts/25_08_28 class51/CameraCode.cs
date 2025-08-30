using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour
{
    /*
     * ========================================
     * ��Ŀ1����Ϸ����������һ�������壬�뽫�����������������ϵλ�ã�ת��Ϊ��Ļ���꣬����ӡ����
     * ========================================
     * 
     * ʵ��˼·��
     * 1. ��ȡ���������������λ�� (transform.position)
     * 2. ʹ������� WorldToScreenPoint() ��������������ת��Ϊ��Ļ����
     * 3. ʹ�� Debug.Log() ��ӡ��Ļ����
     * 
     * �ؼ�֪ʶ�㣺
     * - Camera.WorldToScreenPoint(Vector3 worldPos) ����
     * - ��Ļ����ϵ�����½�Ϊ(0,0)�����Ͻ�Ϊ(Screen.width, Screen.height)
     * - Zֵ��ʾ������������
     */

    /*
     * ========================================
     * ��Ŀ2������Ļ�ϵ��һ����꣬���ڶ�Ӧ����������λ�ô���һ��Cube����
     * ========================================
     * 
     * ʵ��˼·��
     * 1. ���������¼� (Input.GetMouseButtonDown(0))
     * 2. ��ȡ�������Ļ�ϵ�λ�� (Input.mousePosition)
     * 3. ʹ������� ScreenToWorldPoint() ��������Ļ����ת��Ϊ��������
     * 4. �ڸ���������λ��ʵ����һ��Cube (GameObject.CreatePrimitive �� Instantiate)
     * 
     * �ؼ�֪ʶ�㣺
     * - Input.GetMouseButtonDown(0) ������������
     * - Input.mousePosition ��ȡ�����Ļ����
     * - Camera.ScreenToWorldPoint(Vector3 screenPos) ����
     * - ��Ҫ���ú��ʵ�Zֵ��ȷ�����
     * - GameObject.CreatePrimitive(PrimitiveType.Cube) ����������
     */

    // ������Ŀ1������������
    [Header("��Ŀ1 - ����ת��")]
    public GameObject targetCube;

    // ������Ŀ2��CubeԤ����
    [Header("��Ŀ2 - �������")]
    public GameObject cubePrefab;
    
    // ����Cube�ľ��루����������ľ��룩
    public float spawnDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // ���û��ָ�������壬�����ڳ�������һ��
        if (targetCube == null)
        {
            targetCube = GameObject.FindGameObjectWithTag("Player");
            if (targetCube == null)
            {
                // ����һ�������õ�������
                targetCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                targetCube.name = "TestCube";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ŀ1��ʵʱת�����������������Ϊ��Ļ���겢��ӡ
        ConvertWorldToScreenPosition();
        
        // ��Ŀ2�������������ڵ��λ������Cube
        HandleMouseClickToSpawnCube();
    }

    /// <summary>
    /// ��Ŀ1ʵ�֣������������������ת��Ϊ��Ļ���겢��ӡ
    /// </summary>
    private void ConvertWorldToScreenPosition()
    {
        if (targetCube != null)
        {
            // ��ȡ���������������
            Vector3 worldPosition = targetCube.transform.position;
            
            // ����������ת��Ϊ��Ļ����
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            
            // ��ӡ�����ÿ֡��ӡ���Ƶ�������ԼӼ�����ƣ�
            if (Time.frameCount % 60 == 0) // ÿ60֡��ӡһ��
            {
                Debug.Log($"��������������: {worldPosition}, ��Ļ����: {screenPosition}");
            }
        }
    }

    /// <summary>
    /// ��Ŀ2ʵ�֣������������ڶ�Ӧ��������λ������Cube
    /// </summary>
    private void HandleMouseClickToSpawnCube()
    {
        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            // ��ȡ�����Ļ����
            Vector3 mouseScreenPosition = Input.mousePosition;
            
            // ����Zֵ�������������ȣ�
            mouseScreenPosition.z = spawnDistance;
            
            // ����Ļ����ת��Ϊ��������
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            
            // �ڸ�λ������Cube
            GameObject newCube;
            if (cubePrefab != null)
            {
                // �����Ԥ���壬ʹ��Ԥ����
                newCube = Instantiate(cubePrefab, worldPosition, Quaternion.identity);
            }
            else
            {
                // ���򴴽�����������
                newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newCube.transform.position = worldPosition;
            }

            // Ϊ���ɵ�Cube��ӱ�ʶ
            newCube.name = $"ClickedCube_{Time.time:F2}";

            Debug.Log($"����Ļ���� {Input.mousePosition} ��Ӧ���������� {worldPosition}; ������һ��Cube");
        }
    }
}

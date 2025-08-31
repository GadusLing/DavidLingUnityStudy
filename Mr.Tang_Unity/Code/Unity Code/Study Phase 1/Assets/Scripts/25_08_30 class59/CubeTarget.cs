using UnityEngine;

public class CubeTarget : MonoBehaviour
{
    [Header("���������")]
    public int maxHits = 3;           // �����ܴ������
    private int currentHits = 0;      // ��ǰ���������
    
    [Header("�Ӿ�����")]
    public Color[] hitColors = { Color.white, Color.yellow, Color.red }; // ��ͬ�����������ɫ
    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        UpdateVisual();
    }

    /// <summary>
    /// ���ӵ�����
    /// </summary>
    public void OnHit()
    {
        currentHits++;
        Debug.Log($"�����屻���У���ǰ���д���: {currentHits}/{maxHits}");
        
        UpdateVisual();
        
        // ����Ƿ�ﵽ��������
        if (currentHits >= maxHits)
        {
            DestroyCube();
        }
    }

    /// <summary>
    /// �����Ӿ�Ч��
    /// </summary>
    private void UpdateVisual()
    {
        if (cubeRenderer != null && hitColors.Length > currentHits)
        {
            cubeRenderer.material.color = hitColors[currentHits];
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void DestroyCube()
    {
        Debug.Log("�����屻�ݻ٣�");
        // ������������ӱ�ըЧ������Ч��
        Destroy(gameObject);
    }
}
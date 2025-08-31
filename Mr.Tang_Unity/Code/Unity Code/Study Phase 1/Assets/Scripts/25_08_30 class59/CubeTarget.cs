using UnityEngine;

public class CubeTarget : MonoBehaviour
{
    [Header("立方体参数")]
    public int maxHits = 3;           // 最大承受打击次数
    private int currentHits = 0;      // 当前被打击次数
    
    [Header("视觉反馈")]
    public Color[] hitColors = { Color.white, Color.yellow, Color.red }; // 不同打击次数的颜色
    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        UpdateVisual();
    }

    /// <summary>
    /// 被子弹击中
    /// </summary>
    public void OnHit()
    {
        currentHits++;
        Debug.Log($"立方体被击中！当前击中次数: {currentHits}/{maxHits}");
        
        UpdateVisual();
        
        // 检查是否达到销毁条件
        if (currentHits >= maxHits)
        {
            DestroyCube();
        }
    }

    /// <summary>
    /// 更新视觉效果
    /// </summary>
    private void UpdateVisual()
    {
        if (cubeRenderer != null && hitColors.Length > currentHits)
        {
            cubeRenderer.material.color = hitColors[currentHits];
        }
    }

    /// <summary>
    /// 销毁立方体
    /// </summary>
    private void DestroyCube()
    {
        Debug.Log("立方体被摧毁！");
        // 可以在这里添加爆炸效果、音效等
        Destroy(gameObject);
    }
}
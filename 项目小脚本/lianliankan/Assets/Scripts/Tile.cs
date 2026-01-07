using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 方块职责：
// - 持有自己的坐标与类型，所有业务决策交给控制器
// - 点击时只上报事件；清除时仅修改可视与交互状态，不移除对象，保证路径判不会更改（最基础的连连看玩法，棋盘不变）
public class Tile : MonoBehaviour
{
    public int x, y; // 格子坐标，自己在第几行第几列
    public int type; //自己是什么类型 相同 type 才可能消除
    public Button button; // 按钮组件 接收点击动作
    public Image image; // 显示不同图片
    
    public LinkUpGame game; // 游戏控制器（规则与流程都在这里）

    public bool IsCleared => type < 0; // 是否被标记为“已清除” <0则IsCleared为真 表示已清除

    public void Init(int x, int y, int type, LinkUpGame game) // 初始化格子
    {
        this.x = x; // 行坐标
        this.y = y; // 列坐标
        this.type = type; // 类型
        this.game = game; // 控制器引用
        if (button == null) button = GetComponentInChildren<Button>(); // 获取按钮组件，可以拖，没拖的话就自动找
        if (image == null) image = GetComponentInChildren<Image>(); // 获取图片组件，可以拖，没拖的话就自动找
    }

    public void OnClick()
    {
        game.OnTileClick(this);// 告诉控制器我被点了，并把自己的信息传过去
    }

    /// <summary>
    /// 设置选中状态（显示/隐藏发光边框）
    /// </summary>
    public void SetSelected(bool selected)
    {
        // 使用 Unity 自带的 Outline 组件来实现简单的发光效果
        // 注意：UI Outline 性能一般，但在这里足够用了
        var outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.effectColor = Color.yellow; // 发光颜色
            outline.effectDistance = new Vector2(3, -3); // 偏移量，模拟厚度
            outline.useGraphicAlpha = false; // 不受图片透明度影响（主要用于Cleared之后的处理，但这里无关）
        }
        outline.enabled = selected;
    }

    /// <summary>
    /// 播放匹配失败的抖动动画
    /// </summary>
    public void PlayShakeAnimation()
    {
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        // 记录原始的局部坐标。
        //虽 然在 GridLayoutGroup 下改坐标有风险（布局刷新会重置），但短暂的抖动效果通常不会触发布局重算，是可以的。
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        float duration = 0.3f; // 抖动时长
        float magnitude = 5f;  // 抖动幅度（像素）

        while (elapsed < duration)
        {
            // 随机生成一个偏移量
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 还原位置
        transform.localPosition = originalPos;
    }

    // 获取世界坐标下的中心点（用于 LineRenderer 世界空间画线）
    public Vector3 GetWorldCenterPosition()
    {
        // 检查 image 组件是否为空。
        // 虽然 Init 方法里会尝试获取 image，但为了防止意外（比如组件丢失或未初始化），这里做一个安全检查。
        // 如果 image 为空，就退而求其次，直接返回当前 GameObject (transform) 的世界坐标。
        if (image == null) return transform.position;

        // 如果 image 存在，则返回 image 组件对应的 RectTransform 的 position 属性。
        // 在 Unity 中，RectTransform 继承自 Transform，它的 .position 属性获取的就是该 UI 元素在世界空间中的绝对坐标（World Space）。
        // 无论 Canvas 是什么模式（Overlay, Camera, World Space），这个值都是可以直接给 LineRenderer 使用的世界坐标。
        return image.rectTransform.position; 
    }

    public void SetCleared() // 标记格子为已清除
    {
        type = -1; // 标记为已清除
        button.interactable = false; // 禁用按钮点击
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // 直接将透明度置零
        SetSelected(false); // 确保消除后没有残留的边框
    }
}

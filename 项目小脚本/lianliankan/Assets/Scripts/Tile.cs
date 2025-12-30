using System.Collections; // 使用集合命名空间
using System.Collections.Generic; // 使用泛型集合命名空间
using UnityEngine; // 使用UnityEngine
using UnityEngine.UI; // 使用UI命名空间

public class Tile : MonoBehaviour // 单个方块组件
{
    public int x, y; // 网格坐标
    public int type; // 逻辑类型
    public Button button; // 按钮引用
    public Image image; // 图片引用
    public LinkUpGame game; // 游戏控制器引用

    public bool IsCleared => type < 0; // 判定是否已被消除

    public void Init(int x, int y, int type, LinkUpGame game) // 初始化数据
    {
        this.x = x; // 记录行索引
        this.y = y; // 记录列索引
        this.type = type; // 记录方块类型
        this.game = game; // 记录控制器引用
        if (button == null) button = GetComponentInChildren<Button>(); // 兜底绑定按钮
        if (image == null) image = GetComponentInChildren<Image>(); // 兜底绑定图片
    }

    public void OnClick() // 按钮点击回调
    {
        game.OnTileClick(this); // 通知控制器处理点击
    }

    public void SetCleared() // 视觉与交互上标记为空
    {
        type = -1; // 设置为空类型
        button.interactable = false; // 禁用点击
        var c = image.color; // 读取当前颜色
        c.a = 0f; // 隐藏图片
        image.color = c; // 应用颜色
    }
}

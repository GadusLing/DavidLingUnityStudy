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

    public void SetCleared() // 标记格子为已清除
    {
        type = -1; // 标记为已清除
        button.interactable = false; // 禁用按钮点击
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // 直接将透明度置零
    }
}

# 连连看 (LinkUp) 游戏模块使用说明

你好！这是一套开箱即用的连连看逻辑模块。本文档将用最通俗的大白话告诉你怎么把它放进你的游戏里。

---

## 🚀 1. 快速上手（最简单的用法）

### 第一步：准备文件
确保你把 `Scripts` 文件夹里的代码都导进你们的 Unity 工程了。

### 第二步：场景搭建 (UI 拼法)
前端同学请注意，本模块只需要你拼好 UI，逻辑全自动。

1.  **创建容器**：在 Canvas 下创建一个 Panel (或者空物体)，起名叫 `LinkUpPanel`。
2.  **挂载脚本**：把 `LinkUpGame.cs` 脚本拖到这个 Panel 上。
3.  **创建按钮**：
    *   在 Panel 下创建一个 Button，名字必须叫 `btnStart` (用于开始游戏)。
    *   在 Panel 下创建一个 Button，名字必须叫 `btnClose` (用于关闭游戏)。
    *   *偷懒技巧：如果你不想改按钮名字，也可以直接把按钮拖到脚本 Inspector 面板里的 `Start Button` 和 `Close Button` 变量上。*

### 第三步：资源放哪？（默认按这个来）
为了让你不用改代码就能跑，请在 Unity 的 `Assets` 目录下确保有以下文件夹结构：

```text
Assets/
  Resources/
    ├── Tile.prefab      <-- 必需！格子的预制体
    └── Icons/           <-- 必需！放图片的文件夹
          ├── apple.png
          ├── banana.png
          └── ... (随便放多少张图都行，记得把图片设置成Sprite格式)
```

**怎么做 Tile.prefab？**
1.  创建一个 UI Button。
2.  那个 Button 上面应该本来就带着个 Image 组件，这个 Image 就是用来显示水果图案的。
3.  把 `Tile.cs` 脚本挂到这个 Button 上。
4.  拖到 `Resources`文件夹里变成预制体。搞定。

### 第四步：运行
1.  点击 Unity 播放。
2.  你会发现 Start 按钮显示，Close 按钮隐藏（脚本自动控制）。
3.  **点 Start**：游戏开始，网格自动生成，Start 按钮消失，Close 按钮出现。
4.  **点 Close**：一键清空所有格子，瞬间重置回初始状态。

**关于视觉效果：**
*   **发光**：代码会自动给选中的格子加黄色发光边框。
*   **抖动**：匹配失败时格子会自动抖动。
*   *前端不仅不需要写代码，连动画都不用做。*

---

## 🔧 2. 功能配置（想改参数看这里）

点中挂着 `LinkUpGame` 的那个物体，看右边的 Inspector 面板：

*   **Row / Col**: 游戏有几行几列。比如想玩大的就设成 10x10（注意总数必须是偶数，不然消不完）。
*   **Tile Size**: 每一个格子是多大（宽x高）。你可以通过改这个来调整格子的疏密。
*   **Line Duration**: 连线成功后，那条线亮多久（秒）。
*   **Line Width**: 连线的粗细。
*   **Line Color**: 连线的颜色。

---

## 👂 3. 怎么监听“游戏赢了”？

如果你想在游戏胜利时弹个窗或者播个特效，完全不用改代码！

1.  在 Inspector 面板里找到 `On Game Win ()` 这一栏。
2.  点右下角的 `+` 号。
3.  把你做好的胜利弹窗（或者粒子特效）拖进去。
4.  在下拉菜单里选 `GameObject.SetActive` ->打钩（变成 true）。
5.  这样赢了的时候，那个弹窗就会自动弹出来。

当然，如果你是程序，也可以在代码里监听：
```csharp
// 伪代码示例
public LinkUpGame game;

void Start() {
    game.onGameWin.AddListener(OnWin);
}

void OnWin() {
    Debug.Log("我赢啦！发奖金！");
}
```

---

## 🗑️ 4. 怎么销毁游戏？（清理内存）

**问：玩家点关闭按钮(X)了，我也把游戏界面关了，内存会爆吗？**

**答：放心，不会爆。**
我已经加了自动清理功能：
*   当你销毁这个游戏物体（比如调用 `Destroy(gameObject)`）时，脚本会自动触发 `OnDestroy`。
*   它会立刻停止所有的动画协程。
*   它会断开对图片和数据的引用，让垃圾回收机制（GC）能正常工作。

所以，你只需要像平常一样 `Destroy` 掉这个游戏的主界面物体即可。

---

## 📦 5. 高级用法：改为 AB 包加载

如果你们项目比较大，不用 `Resources` 文件夹，而是用 AssetBundle (AB包) 或者 Addressables。

**请修改 `LinkUpGame.cs` 里的 `LoadAssets` 函数：**

```csharp
// 找到这个函数
private void LoadAssets()
{
    // ----------------------------------------------------------------
    // 原始代码（删掉或注释掉）
    // tilePrefab = Resources.Load<GameObject>(tilePrefabPath);
    // tileSprites = Resources.LoadAll<Sprite>(spritesPath);
    // ----------------------------------------------------------------

    // ----------------------------------------------------------------
    // 替换成你们自己的加载代码，比如：
    
    // 1. 加载格子模具
    // tilePrefab = MyResManager.Load<GameObject>("UI/LinkUp/Tile");
    
    // 2. 加载图片（假设你们打成了图集）
    // tileSprites = MyResManager.LoadAtlas("UI/LinkUp/IconAtlas").GetSprites();
    // ----------------------------------------------------------------
}
```
**别忘了相应修改销毁逻辑：**
如果是 AB 包，最好在 `OnDestroy` 里补充一句卸载 AB 包的代码，防止内存泄漏。

```csharp
void OnDestroy()
{
    StopAllCoroutines();
    // ... 其他清理代码 ...

    // 新增：卸载对应的AB包
    // MyResManager.Unload("UI/LinkUp"); 
}
```

# Unity开发.skill.md

## 领域说明
本技能包用于提升AI在Unity 3D游戏开发领域的专业能力，适用于C#和Unity相关的各类项目。

---

## Unity开发知识点
- 推荐使用MonoBehaviour管理游戏对象逻辑，数据容器和共享数据优先用ScriptableObject。
- 善用Unity的事件系统（如Event、Action、UnityEvent）实现解耦。
- UI开发建议使用Unity UI系统（Canvas、RectTransform、Button、Text等）。
- 资源管理建议用Resources、Addressables或AssetBundle。
- 场景管理建议用SceneManager.LoadScene和异步加载。
- 动画推荐用Animator控制器，动画命名用PascalCase。
- 物理系统建议用Rigidbody、Collider，碰撞检测用OnCollisionEnter/OnTriggerEnter。
- 调试建议用Debug.Log、Debug.LogWarning、Debug.LogError，断言用Debug.Assert。
- 推荐用try-catch处理异常，尤其是文件/网络操作。
- 代码风格建议类名用PascalCase，变量/方法用camelCase。

---

## 图片文字提取与自动插入功能

**功能描述：**
- 当用户在对话中输入“copy”、“复制知识点”等指令，并附带一张或多张截图时，AI会自动执行以下操作：
  1. 读取所有截图中的文字内容。
  2. 对提取的文字内容进行适当排版和整理（如分段、加注释符号等）。
  3. 将整理后的内容自动插入到当前工作区的代码文件中（如 .cs 文件），插入位置为用户当前编辑区域。

**使用场景举例：**
- 用户说：“copy知识点”，并上传一张截图，AI会自动将图片中的文字内容以注释或文本形式插入到当前代码区。
- 用户说：“复制这几张图里的内容”，并上传多张截图，AI会依次提取每张图片的文字内容，整理后插入到代码文件中。

**注意事项：**
- 图片中的内容会自动排版为代码注释风格（如 C# 用 // 或 /* */）。
- 若图片内容为多行，AI会自动分段，保持原有结构。
- 若有多张图片，AI会按顺序依次插入内容，并可在每段前加图片序号注释。

---

## 代码模板

```csharp
// MonoBehaviour脚本模板
public class MyScript : MonoBehaviour
{
    void Start() { }
    void Update() { }
}
```

---

## 常见问题与解决方案

- 脚本未挂载：检查是否将脚本拖到GameObject上。
- 资源丢失：检查路径、资源是否打包进项目。
- UI不显示：检查Canvas设置、层级、激活状态。
- 性能卡顿：用Profiler定位瓶颈，优化脚本和资源。

---

## 参考文档

- Unity官方文档：https://docs.unity3d.com/cn/current/Manual/index.html
- Unity API文档：https://docs.unity3d.com/ScriptReference/
- Unity社区与论坛：https://forum.unity.com/

---

> 本技能包可根据项目实际需求扩展，欢迎补充更多Unity开发经验与最佳实践。

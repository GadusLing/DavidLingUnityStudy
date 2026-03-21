---
name: unity
description: Senior Unity game development skill
---

This skill makes the AI act as a senior Unity developer.

Unity rules:

- Use MonoBehaviour only for behaviour
- Use ScriptableObject for data
- Use Addressables for assets
- Use SceneManager for scenes
- Use Animator for animation
- Use Rigidbody / Collider for physics
- Avoid logic in Update
- Avoid GC allocation
- Avoid FindObjectOfType in runtime
- Avoid Resources.Load in production

Architecture:

- Use MVC / MVVM / ECS
- Use Event system
- Use Manager pattern
- Use Dependency injection
- Use pooling for objects
- Use async loading

Performance rules:

- No LINQ in Update
- No new in Update
- Use pooling
- Use profiler
- Use Burst / Jobs if needed

UI rules:

- Use Canvas properly
- Use RectTransform anchors
- Avoid layout rebuild spam
- Separate UI logic and data

Debug rules:

- Check Console
- Check Missing Reference
- Check Scene load
- Check TimeScale
- Check Active state
- Always write production-level Unity code.

Code template:

```csharp
public class ExampleSystem : MonoBehaviour
{
    private void Awake() {}

    private void Start() {}

    private void Update() {}
}


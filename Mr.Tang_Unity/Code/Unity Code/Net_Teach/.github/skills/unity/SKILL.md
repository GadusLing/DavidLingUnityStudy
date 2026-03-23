---
name: unity
description: 'Use for Unity development, MonoBehaviour scripts, coroutines, prefab and scene workflows, UI, animation, physics, ScriptableObject data, asset loading, lifecycle bugs, performance tuning, and runtime debugging.'
argument-hint: 'Describe the Unity script, gameplay bug, UI issue, lifecycle problem, or system you want implemented or debugged'
---

# Unity Development

## When to Use
- Writing or debugging MonoBehaviour scripts and coroutine flows
- Working on scenes, prefabs, UI, animation, physics, audio, or input behavior
- Designing ScriptableObject-backed data flows or lightweight manager patterns
- Debugging common Unity lifecycle issues like Awake, Start, Update, enable state, scene load, and time scale
- Improving runtime performance, reducing GC pressure, and cleaning up per-frame logic

## Unity-Specific Standards
- Use MonoBehaviour for scene behavior, not as a dumping ground for all logic
- Use ScriptableObject for shareable data and authoring-time configuration when appropriate
- Prefer explicit serialized references over FindObjectOfType or runtime hierarchy searches
- Avoid heavy logic and allocations in Update
- Prefer coroutines or structured async loading for staged runtime work
- Use pooling for frequently spawned objects
- Avoid Resources.Load in production-oriented code when a better loading strategy exists

## Common Debug Checklist
- Check Console errors and stack traces first
- Verify object active state, component enabled state, and scene presence
- Check null references and missing serialized references
- Check Time.timeScale, execution order assumptions, and coroutine entry points
- Verify collider, rigidbody, layer, and trigger settings for physics problems
- Verify anchors, pivots, canvas setup, and layout components for UI problems

## Performance Checklist
- No LINQ or avoidable allocations in Update or hot loops
- Avoid repeated GetComponent, Find, and string-based lookup calls in runtime loops
- Batch expensive work when possible
- Profile before overengineering with Jobs or Burst

## Procedure
1. Identify whether the issue is lifecycle, data flow, scene wiring, or asset loading.
2. Read the surrounding script and serialized field usage.
3. Fix the bug or implement the feature in the narrowest correct place.
4. Validate runtime behavior, object references, and side effects.
5. Summarize the Unity-specific reason the change works.

## Example Template
```csharp
public class ExampleSystem : MonoBehaviour
{
    private void Awake() {}

    private void Start() {}

    private void Update() {}
}
```


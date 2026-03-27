---
name: unity
description: 'Use for Unity development, MonoBehaviour scripts, coroutines, prefab and scene workflows, UI, animation, physics, ScriptableObject data, asset loading, lifecycle bugs, performance tuning, runtime debugging, Editor scripting, MenuItem, CustomEditor, EditorWindow, AssetDatabase, and editor tool development.'
argument-hint: 'Describe the Unity script, gameplay bug, UI issue, lifecycle problem, editor tool, or system you want implemented or debugged'
---

# Unity Development

## When to Use
- Writing or debugging MonoBehaviour scripts and coroutine flows
- Working on scenes, prefabs, UI, animation, physics, audio, or input behavior
- Designing ScriptableObject-backed data flows or lightweight manager patterns
- Debugging common Unity lifecycle issues like Awake, Start, Update, enable state, scene load, and time scale
- Improving runtime performance, reducing GC pressure, and cleaning up per-frame logic
- Writing Editor scripts: custom MenuItem tools, EditorWindow, CustomEditor, PropertyDrawer
- Using AssetDatabase API for editor-time asset creation, import, and refresh

## Unity-Specific Standards
- Use MonoBehaviour for scene behavior, not as a dumping ground for all logic
- Use ScriptableObject for shareable data and authoring-time configuration when appropriate
- Prefer explicit serialized references over FindObjectOfType or runtime hierarchy searches
- Avoid heavy logic and allocations in Update
- Prefer coroutines or structured async loading for staged runtime work
- Use pooling for frequently spawned objects
- Avoid Resources.Load in production-oriented code when a better loading strategy exists

## Comment Style
- Mirror the local Unity script style first, especially in lesson scripts, manager scripts, and teaching demos
- Prefer end-of-line comments for field declarations, queue or cache variables, network state, and step-by-step runtime logic when that keeps the script compact and readable
- For coroutine flows, networking flows, asset loading, and parsing logic, comments should connect the stages together: what happened before this line, why this step is necessary now, and what will happen after it succeeds or fails
- Avoid filling MonoBehaviour scripts with generic XML summaries that only restate the method name. If XML comments are used, make them explain usage, Inspector expectations, lifecycle role, or caller-facing contract
- When documenting a helper method, explain how it serves the parent flow. Example: describe that a conversion method exists to feed the loading callback with the requested target type, not just that it "converts a result"
- In Unity learning code, it is acceptable for comments to feel like a mini explanation manual rather than a strict API reference, as long as they remain close to the relevant code

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

## Editor Scripting
- Place all editor-only scripts under `Assets/Editor/` so they do not compile into builds
- Use `[MenuItem("MenuName/ActionName")]` on static methods to add menu entries
- Use `EditorWindow` for custom tool panels; `CustomEditor` for Inspector extensions
- `AssetDatabase.Refresh()` must be called after creating or modifying asset files from code
- `Application.dataPath` returns the Assets folder path; use it for editor-time file I/O
- Editor scripts can reference `UnityEditor` namespace; runtime scripts cannot
- For config-reading tools (XML, JSON, Excel import), keep the reading and generation logic in Editor and output runtime-safe files (C# scripts, ScriptableObjects, JSON)

## Example Template
```csharp
public class ExampleSystem : MonoBehaviour
{
    private void Awake() {}

    private void Start() {}

    private void Update() {}
}
```


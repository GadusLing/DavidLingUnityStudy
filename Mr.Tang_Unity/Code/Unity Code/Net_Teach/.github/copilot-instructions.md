# Project Guidelines

## Workspace Focus
- This workspace is centered on Unity game development using Unity 2022.3.62f2c1.
- Primary runtime language is C#; Lua or hotfix solutions may be introduced later, but C# is the default implementation language.
- Primary UI stack is UGUI. IMGUI may be used for editor tools or temporary internal tooling.
- Primary asset loading direction is AssetBundle-based workflows. Avoid assuming Resources or StreamingAssets are the main production path unless the task explicitly says so.
- Current demo target is PC first. Future-compatible suggestions for Android and iOS are welcome when they do not complicate the PC path too early.

## Response Style
- Do not answer Unity questions with abstract theory only. Provide concrete Unity Editor steps and code steps together.
- When implementing a feature, default to this structure when relevant: goal, Unity Editor setup, scripts to create or modify, Inspector wiring, runtime validation, likely pitfalls.
- Explain why a solution works, but keep the explanation tied to implementation and debugging value.
- Prefer the most proven and stable Unity-friendly solution over overly novel architecture.
- If the user is learning, favor clarity, structure, and step-by-step reasoning over compressed expert shorthand.

## Coding Conventions
- Prefer modern, readable C# and keep code simple.
- Do not force `private` everywhere. Prefer the most appropriate access level for the situation: use `private` or `[SerializeField] private` for stable runtime ownership, but accept `public` fields or methods in small teaching scripts, demos, or when it clearly improves usability and learning flow.
- Private fields may use a leading underscore when it improves clarity.
- Use descriptive English names and prioritize readability over either extreme brevity or unnecessary length.
- Keep comments logical and educational when they add value. Inline comments are acceptable, and structured step comments are encouraged for nontrivial flows when they help the user learn the reasoning.
- `#region` is acceptable when it improves navigation in teaching-style or manager-style scripts.
- Prefer manager-based or MVC-like organization when it fits the existing codebase.

## Unity Implementation Expectations
- For Unity tasks, mention required GameObjects, components, prefabs, scenes, ScriptableObjects, Animator setup, tags, layers, or physics settings when relevant.
- Always mention required Inspector assignments if a script depends on serialized references.
- If a feature depends on import settings, anchors, pivots, colliders, rigidbodies, animation states, or canvas configuration, call that out explicitly.
- For asset loading topics, assume AssetBundle-oriented production workflows unless the user asks for a different loading path.
- For UI topics, assume UGUI first unless the user explicitly asks for UI Toolkit.

## Debugging Expectations
- When diagnosing issues, include both code-side causes and Unity Editor-side causes.
- Check lifecycle order, active state, enabled state, missing references, scene wiring, time scale, physics settings, and UI hierarchy assumptions.
- For bugs, prefer ranked likely causes and concrete verification steps.
- Mention common Unity pitfalls before proposing deeper rewrites.

## Architecture Expectations
- Favor low-coupling, high-cohesion designs that a solo Unity developer can actually maintain.
- Avoid overengineering. Start from a runnable version, then improve extensibility where it matters.
- Use AssetBundle-oriented resource thinking, manager orchestration, and clear ownership of runtime state.
- If recommending a larger system, break it into phases so the user can implement and verify incrementally.

## Model Compatibility
- Write instructions and solutions so they remain useful across lighter and stronger models.
- Prefer explicit steps, trigger words, and concrete checklists over vague high-level guidance.
- Do not rely on hidden assumptions when a short checklist can make the workflow more reliable.
---
name: unity-editor-workflow
description: 'Use for Unity Editor setup steps, GameObject creation, component configuration, Inspector wiring, prefab setup, scene setup, Animator setup, UGUI setup, import settings, and translating a gameplay request into editor actions plus code actions.'
argument-hint: 'Describe the Unity feature or system and ask for both Editor setup and code setup'
---

# Unity Editor Workflow

## When to Use
- The user needs both Unity Editor operations and code changes, not code alone
- A feature depends on creating GameObjects, prefabs, canvases, panels, colliders, layers, tags, or animation assets
- The user asks how to set things up in the Inspector or scene hierarchy
- A system will not work unless Editor-side wiring is done correctly

## Goal
Turn a Unity feature request into a complete implementation workflow that covers scene setup, asset setup, Inspector setup, and code setup from start to finish.

## Required Output Shape
1. Goal and assumptions
2. Unity Editor steps
3. Scripts to create or modify
4. Inspector assignments and required references
5. How to run and verify
6. Common setup mistakes and debugging checks

## Editor Checklist
- Which GameObjects must exist?
- Which components must be added?
- Which serialized fields must be assigned?
- Does the feature depend on tags, layers, sorting layers, colliders, rigidbodies, canvases, anchors, pivots, animation states, or events?
- Does anything need to become a prefab or ScriptableObject asset?

## UGUI Checklist
- Canvas mode and scaler settings
- Anchors and pivots
- Image vs RawImage vs TextMeshPro usage
- Button events and EventSystem presence
- Layout groups, content size fitters, and rebuild risks

## Procedure
1. Identify what must be done in the Editor versus in code.
2. List scene, prefab, and asset setup before writing scripts.
3. Specify exact Inspector bindings for each script.
4. Explain execution order only where it matters.
5. End with a runnable verification path.

## Anti-Patterns
- Giving only scripts when the feature also requires Editor setup
- Forgetting serialized field assignment instructions
- Ignoring anchors, pivots, colliders, tags, layers, or prefab state
- Recommending a flow that cannot be reproduced step by step in the Unity Editor
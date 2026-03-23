---
name: unity-debugging
description: 'Use for Unity bug diagnosis, null references, coroutine issues, lifecycle problems, scene wiring issues, missing Inspector references, UGUI display bugs, physics and trigger bugs, animation state bugs, AssetBundle loading issues, and step-by-step troubleshooting.'
argument-hint: 'Describe the Unity bug, console error, runtime symptom, and reproduction steps'
---

# Unity Debugging

## When to Use
- The user reports a runtime bug, editor bug, console error, missing reference, or unexpected gameplay behavior
- A system compiles but does not behave correctly in scene
- Coroutines, object lifecycle, UI visibility, triggers, animation transitions, or loading flows are failing
- The user needs ranked likely causes instead of random guesses

## Goal
Provide practical Unity debugging guidance that checks both code and Editor configuration, ordered from most likely and cheapest checks to deeper causes.

## Required Output Shape
1. Symptom summary
2. Most likely causes in order
3. What to inspect in Unity Editor
4. What to inspect in code
5. Minimal fix or next experiment
6. How to verify the fix

## High-Frequency Checks
- Null or missing serialized references
- GameObject inactive or component disabled
- Script not on the expected object
- Awake, Start, OnEnable, Update, and coroutine timing assumptions
- Time.timeScale effects
- Wrong scene object, prefab override, or duplicated singleton
- Tag, layer, collider, rigidbody, or trigger mismatch
- Canvas, sorting order, anchors, or raycast target issues
- AssetBundle path, asset name, dependency, or unload timing issues

## Procedure
1. Reconstruct the symptom precisely from logs and behavior.
2. Separate code defects from scene or Inspector misconfiguration.
3. Check the cheapest and most common Unity-specific causes first.
4. Narrow the fault to one system boundary.
5. Suggest the smallest test that confirms or disproves the main hypothesis.

## Bug Classes To Consider
- Lifecycle order bugs
- Coroutine waiting and resume mistakes
- Input not firing
- UI not showing or not receiving clicks
- Physics callbacks not arriving
- Animation controller state mismatch
- AssetBundle and async loading race conditions
- Manager initialization order issues

## Anti-Patterns
- Jumping straight to architecture rewrites for a setup bug
- Ignoring Inspector wiring and scene hierarchy
- Treating Unity runtime symptoms like plain C# console apps
- Offering ten guesses without an order or validation path
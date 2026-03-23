---
name: unity-ui
description: 'Use for Unity UGUI development, Canvas setup, panel flow, Image and RawImage, TextMeshPro, buttons, lists, anchors, pivots, screen adaptation, UI events, UI animation, and step-by-step editor plus code workflow for game UI.'
argument-hint: 'Describe the UI feature, panel, widget, interaction, or layout issue you want implemented or debugged'
---

# Unity UI With UGUI

## When to Use
- Building or debugging Unity UGUI interfaces such as login panels, HUD, inventory, settings, dialog, mission, or battle UI
- Setting up Canvas, EventSystem, panels, buttons, lists, TextMeshPro, Image, RawImage, sliders, toggles, or scroll views
- Handling screen adaptation, anchors, pivots, layout groups, and resolution-related UI issues
- Explaining both the Unity Editor setup and the script-side implementation for a UI feature
- Diagnosing why a UI panel does not show, does not refresh, or does not receive clicks

## Goal
Provide a full UGUI workflow that covers hierarchy design, RectTransform setup, serialized references, event binding, runtime refresh logic, and validation steps.

## Required Output Shape
1. Goal and assumptions
2. Unity hierarchy and Canvas setup
3. Components to add and configure
4. Scripts to create or modify
5. Inspector assignments and event bindings
6. Runtime validation steps
7. Common UGUI pitfalls and fixes

## Editor Checklist
- Is there a Canvas and EventSystem in the scene?
- Which UI objects should be Image, RawImage, TextMeshProUGUI, Button, Toggle, Slider, ScrollRect, or layout group objects?
- Are anchors, pivots, and size deltas set for the intended resolution behavior?
- Are sorting order, raycast targets, masking, and canvas scaler settings correct?
- Should the UI be scene-local, prefab-based, or managed by a UI manager?

## UGUI Rules
- Default to UGUI unless the user explicitly asks for UI Toolkit
- Prefer TextMeshPro for text unless the project says otherwise
- Use Image for Sprite-based UI and RawImage for Texture-based UI
- Avoid layout rebuild spam from careless nesting of layout groups and content fitters
- Keep UI presentation logic separated from broader gameplay logic where practical
- Mention RectTransform anchors and pivots whenever layout correctness matters

## Procedure
1. Define the UI hierarchy and required widgets first.
2. Specify Canvas, scaler, anchor, pivot, and prefab setup in the Editor.
3. Define the script responsibilities: input, view refresh, state sync, and events.
4. List exact Inspector references that must be assigned.
5. End with a concrete play mode verification path.

## Common Pitfalls
- Missing EventSystem or wrong Canvas mode
- Buttons not clickable due to raycast blockers or disabled GraphicRaycaster
- UI not visible due to anchors, scale, sorting order, alpha, or inactive parent objects
- Wrong use of Image versus RawImage
- Text not displaying correctly because TMP components or font assets are missing
- Scroll view content not moving because RectTransform or layout settings are incomplete
- Runtime data changing but UI not refreshing because the binding or refresh entry point is missing

## Output Expectations
- Always combine Unity Editor steps with code steps
- Mention Inspector wiring explicitly
- Prefer stable, proven UGUI patterns over abstract UI architecture lectures
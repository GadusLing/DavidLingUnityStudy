---
name: unity-asset-pipeline
description: 'Use for Unity asset loading, AssetBundle workflows, asset import settings, bundle naming, dependencies, asynchronous loading, caching, unloading, Sprite and texture import setup, streaming and download paths, and resource pipeline design for runtime content.'
argument-hint: 'Describe the asset loading problem, AssetBundle workflow, import setup, or runtime resource pipeline you want designed or debugged'
---

# Unity Asset Pipeline And AssetBundle Workflow

## When to Use
- Designing or debugging AssetBundle-based loading flows
- Deciding how assets should be imported, named, bundled, loaded, cached, and unloaded
- Loading textures, sprites, prefabs, materials, scenes, audio, or config files at runtime
- Explaining the difference between local test paths and production-ready bundle workflows
- Troubleshooting missing dependencies, wrong asset names, broken paths, or unload timing issues

## Goal
Guide Unity asset work from import settings and bundle structure to runtime loading and validation, with AssetBundle workflows treated as the default production path.

## Required Output Shape
1. Goal and assumptions
2. Asset import and Editor preparation steps
3. Bundle structure or loading strategy
4. Scripts or manager changes needed
5. Inspector or config wiring
6. Runtime validation and logging plan
7. Common asset pipeline pitfalls and fixes

## AssetBundle Checklist
- What assets belong in which bundles?
- What dependencies exist across bundles?
- How will bundle names, variants, and asset names be resolved at runtime?
- Where will bundles be stored for local testing versus shipped builds?
- When should bundles or loaded assets be cached and when should they be unloaded?

## Import Settings Checklist
- Texture type, Sprite mode, compression, mipmaps, and read/write settings
- Audio import type and load type
- Model import settings, materials, animations, and scale
- Prefab references and dependency integrity
- Whether the asset is intended for UI, world rendering, audio playback, config parsing, or scene loading

## Runtime Loading Rules
- Prefer AssetBundle-oriented recommendations for production-facing answers in this workspace
- Distinguish between loading the bundle itself and loading assets from the bundle
- Mention asset naming assumptions explicitly
- Mention asynchronous loading and lifecycle ownership when relevant
- Explain unload choices carefully so assets are not released too early or leaked unnecessarily

## Procedure
1. Clarify the target asset type and where it comes from.
2. Specify the required import settings and bundle assignment in the Editor.
3. Define the runtime path strategy for local test, PC build, and future mobile targets.
4. Implement or update the loading manager with explicit error handling and logging.
5. Validate asset names, dependency resolution, and unload timing in play mode.

## Common Pitfalls
- Using the wrong asset name when calling LoadAsset
- Forgetting dependent bundles or manifest handling
- Import settings that do not match runtime usage, especially for Sprite and audio assets
- Mixing toy loading paths with production bundle logic without saying so
- Unloading bundles too early and breaking live references
- Using Chinese names or unstable path conventions in network-delivered assets

## Output Expectations
- Provide both Editor-side asset preparation and runtime code workflow
- Prefer proven AssetBundle patterns that a solo developer can maintain
- Keep the explanation concrete enough that the user can reproduce the pipeline end to end
---
name: gamedev
description: 'Use for game architecture, gameplay systems design, manager design, event systems, save systems, resource pipelines, UI frameworks, networking layers, hotfix architecture, and scalable runtime module planning.'
argument-hint: 'Describe the game system, architecture problem, or framework you want designed or reviewed'
---

# Game Development Architecture

## When to Use
- Designing gameplay frameworks, managers, services, or large feature modules
- Planning resource, save, UI, audio, localization, config, or networking architecture
- Reviewing whether a Unity project is over-coupled, overusing singletons, or mixing responsibilities
- Splitting a large feature into runtime, editor, data, and presentation layers
- Defining extension points for hotfix, scripting, live ops, or DLC-style content

## Core Principles
- Design before coding when a feature spans multiple systems
- Prefer modular structure, low coupling, and high cohesion
- Separate runtime logic, editor tooling, data definitions, and configuration
- Keep UI orchestration separate from core gameplay state
- Prefer composition over inheritance unless inheritance is clearly simpler
- Use interfaces or events when systems should evolve independently

## Architecture Checklist
- What owns state?
- What triggers updates?
- What can be data-driven?
- Which dependencies are runtime-only and which are authoring-time?
- Where should persistence, loading, and error handling live?
- How will the system be extended in 3 months?

## Typical Systems
- UI framework and page flow
- Event bus or message dispatch
- Resource loading and caching
- Save/load pipeline
- Network protocol layer and request handling
- Input abstraction
- Audio routing
- Localization and config tables
- Hotfix or scripting boundary

## Procedure
1. Clarify the gameplay goal and runtime constraints.
2. Define modules and boundaries before writing implementation details.
3. Choose communication style: direct reference, interface, event, or message bus.
4. Minimize circular dependencies and shared mutable state.
5. Only then fill in concrete class design and code.

## Anti-Patterns To Avoid
- Giant manager classes that own unrelated logic
- UI code directly mutating unrelated gameplay systems everywhere
- Hardcoded scene paths, asset paths, and config values spread across code
- Implicit cross-system dependencies with no ownership model
- Premature framework complexity without a concrete use case

## Output Expectations
- Architecture first, code second
- Concrete module boundaries and responsibilities
- Practical designs that fit small or medium Unity projects, not enterprise theater
---
name: csharp
description: 'Use for C# coding, refactoring, bug fixing, API design, generics, async/await, delegates, events, collections, performance tuning, and clean architecture in .NET or Unity C# code.'
argument-hint: 'Describe the C# task, file, bug, or API you want implemented or reviewed'
---

# C# Engineering

## When to Use
- Writing or refactoring C# classes, methods, interfaces, or utilities
- Explaining generics, delegates, events, LINQ, async/await, coroutines, or collections
- Fixing compile errors, null reference risks, type issues, and API misuse
- Improving performance, reducing allocations, and simplifying overly complex code
- Designing reusable systems, managers, services, factories, strategies, and event flows

## Working Style
- Preserve existing public APIs unless the task requires a change
- Prefer simple, explicit code over clever abstractions
- Fix root causes instead of layering patches on top of fragile code
- Keep methods focused and keep responsibilities separated
- Match existing project style unless the current pattern is clearly harmful

## Coding Standards
- Follow SOLID and clean code where they improve maintainability
- Avoid duplicate logic and hidden coupling
- Avoid boxing, unnecessary allocations, and closure capture in hot paths
- Avoid LINQ in performance-sensitive runtime code unless the codebase already relies on it there
- Prefer clear names over abbreviated names
- Prefer readonly, const, and explicit null handling when useful
- Use generic constraints only when they express a real requirement

## Performance Checklist
- Check for per-frame allocations, repeated string concatenation, and temporary collection creation
- Prefer for loops over foreach in hot paths when profiling justifies it
- Avoid repeated GetComponent, Find, reflection, and path parsing in tight loops
- Consider pooling for short-lived objects used frequently
- Use async/await correctly in pure C# code; in Unity runtime code prefer Unity-appropriate async patterns or coroutines

## Procedure
1. Identify the real bug, requirement, or design pressure.
2. Read surrounding code before changing signatures or abstractions.
3. Prefer the smallest coherent fix.
4. Validate edge cases, nullability, and call sites.
5. Summarize behavior changes, risks, and follow-up work if needed.

## Common Patterns
- Factory and strategy for replaceable behavior
- Observer and events for decoupled communication
- Dependency injection or explicit composition for testability
- Command or state objects when branching behavior is becoming tangled

## Output Expectations
- Production-quality C# code
- Clear reasoning about tradeoffs
- Safe refactors with minimal collateral changes
---
name: csharp
description: 'Use for C# coding, refactoring, bug fixing, API design, generics, async/await, delegates, events, collections, performance tuning, clean architecture, reflection, FieldInfo, Type system, StringBuilder, code generation, serialization, byte arrays, BitConverter, abstract classes, interfaces, and design patterns in .NET or Unity C# code.'
argument-hint: 'Describe the C# task, file, bug, or API you want implemented or reviewed'
---

# C# Engineering

## When to Use
- Writing or refactoring C# classes, methods, interfaces, or utilities
- Explaining generics, delegates, events, LINQ, async/await, coroutines, or collections
- Fixing compile errors, null reference risks, type issues, and API misuse
- Improving performance, reducing allocations, and simplifying overly complex code
- Designing reusable systems, managers, services, factories, strategies, and event flows
- Using reflection (GetType, GetFields, FieldInfo, Activator.CreateInstance, BindingFlags)
- Building code generators that assemble C# source files from config data using StringBuilder
- Working with byte-level serialization (BitConverter, Encoding.UTF8, byte arrays, index cursors)

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

## Comment Style
- Match the surrounding file's comment style before adding new comments
- For teaching scripts or manager classes, prefer compact end-of-line comments after the code statement when the explanation is short or medium length
- Do not default to placing a separate comment line above every simple statement; that often makes the code feel fragmented
- Comments should explain intent and flow, not just translate the code literally. Prefer: why this is done here, what earlier condition caused it, and what later behavior it enables
- When a method needs higher-level explanation, describe how it serves the caller or the overall pipeline, not only a dictionary-style summary of the method name
- Use `///` XML comments mainly for public methods, reusable APIs, or signatures where parameters and return values need explicit guidance
- If the file already reads like a tutorial, write comments like a walkthrough so adjacent lines feel connected rather than isolated

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

## Reflection Patterns
- Use `GetType().GetFields(BindingFlags)` to collect fields; always specify BindingFlags explicitly
- `BindingFlags.Instance | BindingFlags.Public` for public instance fields only
- `Activator.CreateInstance(type)` to dynamically create instances from a Type; use for deserialization and factory patterns
- `FieldInfo.GetValue(obj)` / `FieldInfo.SetValue(obj, value)` to read/write fields reflectively
- `type.IsSubclassOf(baseType)`, `type.IsGenericType`, `type.GetGenericTypeDefinition()`, `type.GetGenericArguments()` for type inspection
- `type.IsArray` and `type.GetElementType()` for array type inspection
- Reflection is slow per call; avoid in per-frame hot paths; cache results when called repeatedly

## Code Generation Patterns
- Use `StringBuilder` for assembling generated code; avoid string concatenation in loops
- Build generated files top-down: using statements → namespace → class declaration → members → closing braces
- Always write generated files with `File.WriteAllText(path, content)` and call `AssetDatabase.Refresh()` in editor tools
- Use consistent indentation (4 spaces or 1 tab) in generated code for readability
- Include a comment header in generated files marking them as auto-generated so they are not manually edited

## Common Patterns
- Factory and strategy for replaceable behavior
- Observer and events for decoupled communication
- Dependency injection or explicit composition for testability
- Command or state objects when branching behavior is becoming tangled

## Output Expectations
- Production-quality C# code
- Clear reasoning about tradeoffs
- Safe refactors with minimal collateral changes
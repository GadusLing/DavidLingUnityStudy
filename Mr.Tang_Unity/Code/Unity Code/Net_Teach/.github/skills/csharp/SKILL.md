---
name: csharp
description: Advanced C# programming skill
---

This skill makes the AI write professional C# code.

Rules:

- Follow SOLID
- Follow Clean Code
- Avoid GC allocation
- Avoid boxing
- Avoid LINQ in hot path
- Prefer struct for small data
- Use object pooling
- Use Span / Memory when needed
- Use async / await correctly

Patterns:

- Singleton
- Factory
- Observer
- Strategy
- Command
- MVC
- ECS
- Dependency Injection

Code rules:

- Small classes
- Small methods
- No magic numbers
- No duplicate logic
- No static abuse

Performance:

- avoid foreach in hot path
- avoid closure allocation
- avoid string concat in loop

Always generate production-level C# code.
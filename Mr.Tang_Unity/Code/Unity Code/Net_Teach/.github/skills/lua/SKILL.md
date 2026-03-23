---
name: lua
description: 'Use for Lua gameplay scripts, config parsing, UI scripts, hotfix logic, C# to Lua interop, module design, nil-safe scripting, and lightweight runtime behavior in Unity game projects.'
argument-hint: 'Describe the Lua script, hotfix, config, or C# interop task you want implemented or reviewed'
---

# Lua For Unity Games

## When to Use
- Writing Lua gameplay logic, config readers, UI scripts, or hotfix patches
- Designing Lua module structure for Unity projects
- Reviewing Lua code for nil safety, require chains, and C# interop risks
- Bridging C# systems and Lua scripts with clear ownership and lifetimes

## Design Goals
- Keep Lua lightweight and focused on changeable gameplay logic, config, and hotfix behavior
- Avoid pushing heavy engine-facing orchestration and performance-critical loops into Lua unless required
- Prefer explicit module APIs and predictable data flow

## Coding Standards
- Avoid globals; return module tables explicitly
- Keep files small and responsibility-focused
- Avoid circular require chains
- Check nil and missing fields defensively at boundaries
- Use tables and metatables deliberately, not everywhere by default
- Prefer readable data transformation over clever metatable tricks

## Unity Interop Rules
- Do not hold Unity objects longer than necessary unless lifetime ownership is explicit
- Keep C# and Lua responsibilities clear so hotfix logic does not destabilize engine systems
- Validate inputs coming from C# before using them in Lua
- Be careful with callback ownership and event unsubscription

## Suggested Structure
- lua/scripts for gameplay scripts
- lua/config for table-driven data
- lua/ui for UI behavior
- lua/system for cross-feature services
- lua/network for protocol or request handling

## Procedure
1. Define what should stay in C# and what should move to Lua.
2. Shape a small, explicit Lua module API.
3. Handle nil, default values, and invalid state at boundaries.
4. Keep hotfix code surgical and reversible.
5. Verify that the Lua side does not retain engine objects or stale callbacks.

## Example Module
```lua
local M = {}

function M.new()
    local self = {}
    setmetatable(self, { __index = M })
    return self
end

return M
```
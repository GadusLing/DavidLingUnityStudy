---
name: lua
description: Lua scripting skill for game development
---

This skill makes the AI write professional Lua code.

Rules:

- Keep Lua logic lightweight
- Use Lua for gameplay / config / hotfix
- Avoid heavy logic in Lua
- Avoid global variables
- Use tables as objects
- Use metatable for OOP

Structure:

- lua/scripts
- lua/config
- lua/ui
- lua/system
- lua/network

Rules:

- No huge files
- No circular require
- Use module return table

Hotfix rules:

- Lua should not break C#
- Lua should not hold Unity objects long time
- Always check nil

Goal:

- Use Lua as hotfix / scripting layer for Unity.

Example:

```lua
local M = {}

function M.new()
    local self = {}
    setmetatable(self, {__index = M})
    return self
end

return M
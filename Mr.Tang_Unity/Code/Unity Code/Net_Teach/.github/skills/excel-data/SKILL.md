---
name: excel-data
description: 'Use for Excel data import, reading xlsx/xls/csv files in Unity Editor tools, config table workflows, Excel to C# class generation, Excel to ScriptableObject pipelines, EPPlus or NPOI usage, designer-facing data editing tools, and batch data import for game configs.'
argument-hint: 'Describe the Excel file, config table, data import pipeline, or editor tool you want implemented or reviewed'
---

# Excel Data Handling For Unity

## When to Use
- Importing game config data from Excel (.xlsx/.xls) or CSV files into Unity
- Building editor tools that let designers edit configs in Excel and auto-import into the game
- Generating C# data classes or ScriptableObjects from Excel table definitions
- Designing a config table pipeline: Excel → intermediate format → runtime data
- Parsing CSV files as a lightweight alternative to full Excel parsing
- Batch processing multiple Excel sheets into different config categories

## Pipeline Overview
Excel is an authoring tool, not a runtime format. The standard workflow:
1. Designer edits data in Excel (human-friendly columns and rows)
2. An Editor tool reads Excel and converts to a runtime-friendly format (C# class, JSON, binary, ScriptableObject)
3. Runtime code loads the converted format, never reads Excel directly

## Library Options

### EPPlus (recommended for .xlsx)
- NuGet: EPPlus; works well in Unity Editor scripts
- `ExcelPackage`, `ExcelWorksheet`, `ExcelRange` are the core types
- Read cell: `worksheet.Cells[row, col].Value`
- Supports formulas, formatting, multiple sheets
- Must be used in Editor code only (not in builds)

### NPOI (alternative, supports .xls and .xlsx)
- More complex API but handles legacy .xls format
- `HSSFWorkbook` for .xls, `XSSFWorkbook` for .xlsx

### CSV (simplest, no library needed)
- `File.ReadAllLines(path)` then split each line by comma
- Good enough for simple flat tables without formulas or merged cells
- Watch for: commas inside quoted fields, encoding issues, empty cells

## Editor Tool Procedure
1. Place the Excel reading code in `Assets/Editor/` so it does not ship with builds.
2. Add a `[MenuItem]` entry for designers to trigger the import.
3. Read the Excel file, iterate rows and columns.
4. Row 0 or 1 is usually the header (field names); row 1 or 2 is the type row; data starts after.
5. For each data row, map columns to a C# object or generate a config entry.
6. Output the result: write to JSON, ScriptableObject, or generated C# class file.
7. Call `AssetDatabase.Refresh()` to make Unity recognize new/changed assets.

## Config Table Design Rules
- First row: field names (English, matching C# field names exactly)
- Second row: field types (int, float, string, bool, List<int>, etc.)
- Third row onward: actual data values
- One sheet per config category (items, monsters, skills, levels, etc.)
- Use int IDs as primary keys for runtime lookup (Dictionary<int, ConfigData>)

## Common Pitfalls
- Including EPPlus/NPOI DLLs outside of Editor folder — causes build errors on mobile
- Cell values returning as object type — always null-check and Convert/Parse explicitly
- Merged cells in Excel causing null reads for covered cells
- Date columns being read as Excel serial numbers instead of DateTime
- Not handling empty rows or trailing empty cells at the end of a sheet
- Path issues: use `Application.dataPath` for editor-time paths; designers may put Excel anywhere
- Forgetting to strip BOM or handle UTF-8 encoding for Chinese content in CSV

## Output Expectations
- Always separate editor import code from runtime data code
- Show the expected Excel column layout alongside the C# data class
- Mention which library is being used and where to put the DLL
- Include AssetDatabase.Refresh() after file generation

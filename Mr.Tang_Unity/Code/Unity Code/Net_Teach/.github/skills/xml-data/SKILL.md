---
name: xml-data
description: 'Use for XML parsing, XmlDocument, XmlNode, XmlNodeList, XmlAttribute, SelectSingleNode, SelectNodes, XPath queries, reading XML config files, generating C# code from XML definitions, protocol XML config design, game config XML structure, and XML file I/O in Unity C# projects.'
argument-hint: 'Describe the XML file, parsing task, config structure, or code generation workflow you want implemented or reviewed'
---

# XML Data Handling In Unity C#

## When to Use
- Parsing XML config files with XmlDocument, XmlNode, or XmlNodeList
- Reading XML attributes and child nodes with SelectSingleNode or SelectNodes
- Designing XML structure for protocol definitions, enum configs, data class configs, or message configs
- Generating C# code (enums, classes, fields) from XML definitions
- Writing or updating XML files from editor tools
- Debugging XML parsing issues: wrong node names, missing attributes, encoding problems
- Working with XPath queries to select specific XML elements

## Core API Reference
- `XmlDocument.Load(path)` — load XML file from disk; path must be absolute or resolved correctly
- `XmlDocument.SelectSingleNode(xpath)` — get one node matching XPath; returns null if not found
- `XmlNode.SelectNodes(name)` — get all direct-child or descendant nodes matching the name
- `XmlNode.Attributes["name"].Value` — read a named attribute's string value; throws if attribute missing
- `XmlNode.InnerText` — the text content between opening and closing tags
- `XmlNode.ChildNodes` — all direct children regardless of name (less precise than SelectNodes)
- `XmlNodeList` — iterable collection returned by SelectNodes; use foreach to iterate

## XML Config Design Rules
- Use element names to express node type (enum, data, message, field)
- Use attributes for metadata: name, namespace, id, type
- Use InnerText only for simple scalar values like enum field values
- Keep hierarchy shallow: root → category nodes → field nodes
- Prefer SelectNodes("fieldName") over ChildNodes when you only want specific child types
- Always null-check attributes before accessing .Value to avoid NullReferenceException

## Code Generation From XML Procedure
1. Load the XML file with `XmlDocument.Load(absolutePath)`.
2. Get the root node with `SelectSingleNode("rootName")`.
3. Get category node lists with `root.SelectNodes("enum")`, `root.SelectNodes("data")`, etc.
4. For each category node, read attributes (name, namespace) first.
5. Get field nodes with `categoryNode.SelectNodes("field")`.
6. For each field, read type/name/value attributes and build the C# code string.
7. Use StringBuilder to assemble the output file content efficiently.
8. Write the generated .cs file with `File.WriteAllText(path, content)`.
9. Call `AssetDatabase.Refresh()` in editor tools to make Unity recognize new files.

## Type Mapping (XML → C#)
- When XML field type is a known primitive (int, float, string, bool, short, long, byte), map directly
- When XML field type is "list", read the element-type attribute (e.g., data="int") → `List<int>`
- When XML field type is "dictionary", read key-type and value-type attributes → `Dictionary<K,V>`
- When XML field type is "array", read the element-type attribute → `int[]`
- When XML field type matches a known data class name, use that class name directly

## Common Pitfalls
- Forgetting that `SelectNodes` returns nodes by name, while `ChildNodes` returns everything including whitespace text nodes in some parsers
- Not checking if `Attributes["x"]` is null before calling `.Value` — will throw NullReferenceException
- Using relative paths in `XmlDocument.Load()` — always use `Application.dataPath` based absolute paths in Unity
- Encoding mismatch: ensure XML files are saved as UTF-8; check `<?xml version="1.0" encoding="UTF-8"?>` header
- Forgetting `AssetDatabase.Refresh()` after generating files from editor tools
- String concatenation in loops for code generation; prefer StringBuilder

## Output Expectations
- Always show the XML structure and the C# parsing code together
- When generating code from XML, show both the XML source format and the expected C# output
- Mention null-safety checks for attributes and nodes explicitly

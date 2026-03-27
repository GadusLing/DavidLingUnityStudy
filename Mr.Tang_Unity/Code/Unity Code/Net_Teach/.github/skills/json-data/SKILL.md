---
name: json-data
description: 'Use for JSON parsing, JsonUtility, JsonConvert, Newtonsoft Json.NET, reading and writing JSON config files, save data serialization, server response parsing, JSON file I/O, JSON to C# object mapping, and JSON data handling in Unity C# projects.'
argument-hint: 'Describe the JSON file, parsing task, data mapping, or serialization workflow you want implemented or reviewed'
---

# JSON Data Handling In Unity C#

## When to Use
- Parsing JSON config files or server responses into C# objects
- Serializing C# objects to JSON for save files, config export, or network payloads
- Choosing between JsonUtility and Newtonsoft Json.NET for a specific use case
- Designing JSON data structures for game configs, save data, or communication protocols
- Debugging JSON parse failures: format errors, missing fields, type mismatches
- Converting between JSON and ScriptableObject or plain C# data classes

## API Quick Reference

### Unity JsonUtility (built-in, zero dependency)
- `JsonUtility.ToJson(obj, prettyPrint)` — serialize a C# object to JSON string
- `JsonUtility.FromJson<T>(jsonString)` — deserialize JSON string to a new instance of T
- `JsonUtility.FromJsonOverwrite(jsonString, existingObj)` — fill an existing object from JSON

### JsonUtility Limitations (important)
- Does NOT support Dictionary — use List of key-value pairs or switch to Json.NET
- Does NOT support polymorphism — cannot deserialize base-class references to derived types
- Only serializes public fields and [SerializeField] private fields (same rules as Unity serialization)
- Does NOT support top-level arrays — wrap in a container class: `{ "items": [...] }`
- Enum values serialized as int by default, not string names

### Newtonsoft Json.NET (when available via package or DLL)
- `JsonConvert.SerializeObject(obj, Formatting.Indented)` — full-featured serialization
- `JsonConvert.DeserializeObject<T>(jsonString)` — full-featured deserialization
- Supports Dictionary, polymorphism, custom converters, enum as string, nullable types
- Add via Unity Package Manager: com.unity.nuget.newtonsoft-json

## When to Use Which
- **JsonUtility**: simple flat data, save files with only public fields, no Dictionary needed, zero dependency preferred
- **Json.NET**: complex data with Dictionary, polymorphism, custom formatting, or server API integration

## JSON File I/O In Unity
- Read: `string json = File.ReadAllText(path);` then parse
- Write: `File.WriteAllText(path, jsonString);`
- Common paths:
  - `Application.dataPath` — Assets folder (editor only, not in builds)
  - `Application.persistentDataPath` — writable at runtime on all platforms
  - `Application.streamingAssetsPath` — read-only shipped data

## Procedure
1. Decide if JsonUtility or Json.NET is needed based on data complexity.
2. Define the C# data class with public fields matching JSON keys exactly (case-sensitive).
3. For JsonUtility: ensure fields are public or marked [SerializeField]; no Dictionary or inheritance.
4. Load the JSON string from file or network response.
5. Deserialize into the target type and validate critical fields are not null/default.
6. For save data: serialize with ToJson, write to persistentDataPath, handle IO exceptions.

## Common Pitfalls
- JSON key names not matching C# field names exactly (case-sensitive)
- Using JsonUtility with Dictionary fields — silently ignored, no error, just empty data
- Forgetting that JsonUtility requires a wrapping object for top-level arrays
- File path issues: using dataPath at runtime in builds (does not exist); use persistentDataPath instead
- Not handling FileNotFoundException or malformed JSON gracefully
- Enum serialized as int by JsonUtility but as string by Json.NET — causes mismatch when mixing
- Forgetting [Serializable] attribute on the data class when using JsonUtility

## Output Expectations
- Show the C# data class and the JSON structure side by side
- Mention which JSON library is being used and why
- Include file I/O code when the task involves reading/writing files
- Mention platform path differences when the task targets builds, not just editor

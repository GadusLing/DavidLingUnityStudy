# AI Coding Agent Guidelines for the Unity UI_UGUI_Demo Project

## Project Overview
This project is a Unity-based application focused on demonstrating UI development using Unity's UGUI framework. The codebase includes various components for managing UI elements, scenes, and game logic. It integrates Unity packages such as TextMeshPro, Visual Scripting, and Test Tools.

## Key Directories
- **Assets/Scripts/**: Contains all the C# scripts for game logic and UI management.
- **Assets/Scenes/**: Stores Unity scene files.
- **Assets/Resources/**: Holds resources loaded dynamically at runtime.
- **Assets/TextMesh Pro/**: Includes assets and configurations for TextMeshPro.

## Architecture
- **UI Management**: Scripts in `Assets/Scripts/Login/` and similar directories manage specific UI components.
- **Scene Management**: Unity scenes are organized under `Assets/Scenes/`.
- **External Dependencies**: The project references Unity packages like `UnityEngine.UI`, `UnityEditor.UI`, and `TextMeshPro`.

## Developer Workflows
### Building the Project
1. Open the project in Unity Editor.
2. Use the "Build Settings" menu to configure the target platform.
3. Click "Build and Run" to compile and execute the project.

### Running Tests
- Tests are integrated using Unity Test Framework.
- To run tests, open the Test Runner window in Unity Editor and execute the desired test suite.

### Debugging
- Use Unity's built-in debugger or attach an external debugger (e.g., Visual Studio).
- Logs are written to the Unity Console.

## Project-Specific Conventions
- **Namespace Usage**: Scripts are organized under namespaces matching their directory structure.
- **Prefab Usage**: UI elements are modularized as prefabs for reusability.
- **Coding Style**: Follow Unity's C# coding conventions.

## Integration Points
- **TextMeshPro**: Used for advanced text rendering.
- **Unity Visual Scripting**: Enables visual programming for non-coders.
- **Unity Test Tools**: Provides a framework for writing and running tests.

## Examples
### UI Manager Pattern
```csharp
namespace Login
{
    public class LoginMgr : MonoBehaviour
    {
        void Start()
        {
            // Initialize UI components
        }
    }
}
```

### Dynamic Resource Loading
```csharp
var resource = Resources.Load("path/to/resource");
```

## Notes for AI Agents
- Maintain compatibility with Unity 2022.3 or later.
- Ensure changes do not break existing prefabs or scenes.
- Follow the directory structure and naming conventions strictly.

For further questions, refer to Unity's official documentation or the Unity Editor's built-in help system.
# Flags
This project implements a flexible flag system designed to manage and serialize flags using C#. The system is composed of several components, each fulfilling a specific role within the architecture. The flag system can be useful in scenarios where objects need to be marked or tagged with a set of flags, such as in feature toggles, permissions, or other conditional logic.
## Usage
```csharp
using Cencora.TransportWeb.Common.Flags;

public class MyClass
{
    public FlagContainer Flags = new();
}

public class MyOtherClass
{
    public void Sample()
    {
        MyClass test = new MyClass();
        MyClass.Flags.AddFlag(new Flag("Open"))

        bool hasOpenFlag = test.Flags.HasFlag("Open");
    }
}
```
## Features
- Flexible Flag Declaration: Flags can be explicitly declared using the `new Flag("Value")` syntax or directly from a string, such as `Flag flag = "Value"`. This flexibility allows for more intuitive and readable code.
- Comprehensive Flag Management: The FlagContainer class provides a comprehensive API for managing flags, including methods to add, remove, and check for the presence of specific flags.
- Serialization Support: The flag system includes built-in support for serialization to and from JSON, making it easy to persist and transfer flag states across different layers of an application.
- Use Cases: Ideal for implementing feature toggles, managing user permissions, and other scenarios where conditional logic based on flags is required.

This flag system is designed to be both powerful and easy to use, providing developers with the tools they need to implement complex flag-based logic in a clean and maintainable way.
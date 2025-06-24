# SimpleBlackboard.Net

![Logo](https://raw.githubusercontent.com/vonderborch/SimpleBlackboard.Net/refs/heads/main/logo.png)

A simple package containing a basic Blackboard data structure.

## Installation

### Nuget

[![NuGet version (SimpleBlackboard.Net)](https://img.shields.io/nuget/v/SimpleBlackboard.Net.svg?style=flat-square)](https://www.nuget.org/packages/SimpleBlackboard.Net/)

The recommended installation approach is to use the available nuget package: [SimpleBlackboard.Net](https://www.nuget.org/packages/SimpleBlackboard.Net/)

### Clone

Alternatively, you can clone this repo and reference the SimpleBlackboard.Net project in your project.

## Features

- A Blackboard interface that can be extended to create custom blackboards (`IBlackboard`).
- A standard Blackboard class that can be used out-of-the-box (`Blackboard`)

## What is a Blackboard?

A Blackboard is a data structure designed to act as a structured global memory containing objects from the solution
space. For more information, see:
- https://architectural-patterns.net/blackboard
- https://en.wikipedia.org/wiki/Blackboard_(design_pattern)

## Usage

### Built-in Blackboard

The built-in `Blackboard` class provides a ready-to-use _very simplified_ implementation that can be used out-of-the-box
. This version relies on an internal dictionary to store the data and is agnostic about implementation details that may
be important for your specific implementation.

#### Example Usage:

```csharp
using SimpleBlackboard.Net;

public class Program
{
    public static void Main()
    {
        // Create a new blackboard (optionally with thread-safe concurrent dictionary)
        var blackboard = new Blackboard(useConcurrentDictionary: true);

        // Store different types of data
        blackboard.SetValue("PlayerName", "John");
        blackboard.SetValue("PlayerHealth", 100);
        blackboard.SetValue("IsAlive", true);
        blackboard.SetValue("Position", new Vector3(10.5f, 0, 3.2f));

        // Retrieve data in a type-safe manner
        string name = blackboard.GetValue<string>("PlayerName");
        int health = blackboard.GetValue<int>("PlayerHealth");

        // Check if a value exists
        if (blackboard.HasValue<bool>("IsAlive"))
        {
            Console.WriteLine($"Player {name} is alive: {blackboard.GetValue<bool>("IsAlive")}");
        }

        // Try to get a value safely
        if (blackboard.TryGetValue("Position", out Vector3? position))
        {
            Console.WriteLine($"Player position: {position}");
        }

        // Remove a value and get it at the same time
        int oldHealth = blackboard.RemoveValue<int>("PlayerHealth") ?? 0;

        // Try to remove a value safely
        if (blackboard.TryRemoveValue("IsAlive", out bool isAlive))
        {
            Console.WriteLine($"Removed 'IsAlive' flag: {isAlive}");
        }

        // Clear all data from the blackboard
        blackboard.ClearBlackboard();
    }
}
```

### Custom Blackboard

Below is a simple blackboard implementation. However, besides implementing the standard fields, you have full control as
to the structure of the Blackboard and it would be **highly** recommended to have and use structured fields/properties additionally
to the built-in methods.

#### Example Usage:

```csharp

using SimpleBlackboard.Net;

public class MyBlackboard : IBlackboard
{
    private readonly Dictionary<string, object?> _data = new();
    
    public bool SetValue<T>(string key, T value)
    {
        _data[key] = value;
        return true;
    }
    
    public T? GetValue<T>(string key)
    {
        if (_data.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
    
    public bool TryGetValue<T>(string key, out T? value)
    {
        if (_data.TryGetValue(key, out var typedValue) && typedValue is T typedValueT)
        {
            value = typedValueT;
            return true;
        }

        value = default;
        return false;
    }
    
    public bool HasValue<T>(string key)
    {
        return this._data.ContainsKey(key);
    }
    
    public void ClearBlackboard()
    {
        _data.Clear();
    }
    
    public T? RemoveValue<T>(string key)
    {
        if (TryGetValue(key, out T? value))
        {
            _data.Remove(key);
            return value;
        }

        return default;
    }
    
    public bool TryRemoveValue<T>(string key, out T? value)
    {
        if (_data.TryGetValue(key, out var typedValue) && typedValue is T typedValueT)
        {
            value = typedValueT;
            return _data.Remove(key);
        }

        value = default;
        return false;
    }
}

public class Program
{
    public static void Main()
    {
        var blackboard = new MyBlackboard();
        
        // Set a value
        blackboard.SetValue("Score", 100);
        
        // Get a value
        int score = blackboard.GetValue<int>("Score");
        Console.WriteLine($"Score: {score}");
        
        // Check if a value exists
        bool hasScore = blackboard.HasValue<int>("Score");
        Console.WriteLine($"Has Score: {hasScore}");
        
        // Clear the blackboard
        blackboard.ClearBlackboard();
    }
}

```

## Development

1. Clone or fork the repo
2. Create a new branch
3. Code!
4. Push your changes and open a PR
5. Once approved, they'll be merged in
6. Profit!

## Future Plans

See list of issues under the Milestones: https://github.com/vonderborch/SimpleBlackboard.Net/milestones

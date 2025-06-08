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

- An abstract Blackboard class that can be extended to create custom blackboards.

## Usage

```csharp

using SimpleBlackboard.Net;

public class MyBlackboard : Blackboard
{
    private readonly Dictionary<string, object> _data = new();

    public override bool SetValue<T>(string key, T value)
    {
        _data[key] = value!;
        return true;
    }

    public override T GetValue<T>(string key)
    {
        if (_data.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
    
    public override bool HasValue<T>(string key)
    {
        return this._data.ContainsKey(key);
    }
    
    public override void ClearBlackboard()
    {
        _data.Clear();
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

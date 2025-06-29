using System.Collections.Concurrent;

namespace SimpleBlackboard.Net;

/// <summary>
/// Represents a data store that associates keys with values of various types, allowing storage, retrieval, and removal
/// of values in a generic and type-safe manner.
/// </summary>
public class Blackboard : IBlackboard
{
    /// <summary>
    /// Represents a private dictionary used to store key-value pairs in the blackboard.
    /// Keys are strings, and values can be of any object type, including null.
    /// </summary>
    private readonly IDictionary<string, object?> _data;

    /// <summary>
    /// Represents a data store that allows associating keys with values of various types.
    /// Provides methods for adding, retrieving, removing, and checking for values in a type-safe way.
    /// </summary>
    /// <param name="useConcurrentDictionary">True to use a concurrent dictionary under the hood, False to use a regular dictionary.</param>
    public Blackboard(bool useConcurrentDictionary = false)
    {
        _data = useConcurrentDictionary
            ? new ConcurrentDictionary<string, object?>()
            : new Dictionary<string, object?>();
    }
    
    /// <summary>
    /// Sets a value in the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">The value to store in the blackboard.</param>
    /// <returns>True if the value was successfully set; otherwise, false.</returns>
    public bool SetValue<T>(string key, T value)
    {
        _data[key] = value;
        return true;
    }

    /// <summary>
    /// Retrieves a value from the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <returns>The value associated with the specified key.</returns>
    public T? GetValue<T>(string key)
    {
        if (_data.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    /// <summary>
    /// Attempts to retrieve a value from the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <param name="value">
    /// When this method returns, contains the value associated with the specified key
    /// if the operation was successful; otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>True if a value of the specified type exists and was successfully retrieved; otherwise, false.</returns>
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

    /// <summary>
    /// Checks if the blackboard contains a value with the specified key of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to check.</typeparam>
    /// <param name="key">The key to check for existence in the blackboard.</param>
    /// <returns>True if a value of the specified type exists with the given key; otherwise, false.</returns>
    public bool HasValue<T>(string key)
    {
        return this._data.ContainsKey(key);
    }

    /// <summary>
    /// Removes all values from the blackboard, resetting it to an empty state.
    /// </summary>
    public void ClearBlackboard()
    {
        _data.Clear();
    }
    
    /// <summary>
    /// Removes a value from the blackboard associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to remove.</typeparam>
    /// <param name="key">The key associated with the value to be removed.</param>
    /// <returns>The value that was removed from the blackboard.</returns>
    public T? RemoveValue<T>(string key)
    {
        if (TryGetValue(key, out T? value))
        {
            _data.Remove(key);
            return value;
        }

        return default;
    }
    
    /// <summary>
    /// Attempts to remove a value with the specified key from the blackboard.
    /// </summary>
    /// <typeparam name="T">The type of the value to remove.</typeparam>
    /// <param name="key">The key associated with the value to remove.</param>
    /// <param name="value">The removed value if the key exists and the type matches; otherwise, the default value of <typeparamref name="T"/>.</param>
    /// <returns>True if the value was successfully removed; otherwise, false.</returns>
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

namespace SimpleBlackboard.Net;

/// <summary>
/// Abstract base class for implementing a blackboard data structure.
/// A blackboard is a centralized data storage mechanism for sharing information between components.
/// </summary>
public abstract class Blackboard
{
    /// <summary>
    /// Sets a value in the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">The value to store in the blackboard.</param>
    /// <returns>True if the value was successfully set; otherwise, false.</returns>
    public abstract bool SetValue<T>(string key, T value);

    /// <summary>
    /// Retrieves a value from the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <returns>The value associated with the specified key.</returns>
    public abstract T GetValue<T>(string key);

    /// <summary>
    /// Checks if the blackboard contains a value with the specified key of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to check.</typeparam>
    /// <param name="key">The key to check for existence in the blackboard.</param>
    /// <returns>True if a value of the specified type exists with the given key; otherwise, false.</returns>
    public abstract bool HasValue<T>(string key);

    /// <summary>
    /// Removes all values from the blackboard, resetting it to an empty state.
    /// </summary>
    public abstract void ClearBlackboard();
}

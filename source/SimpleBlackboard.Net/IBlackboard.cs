namespace SimpleBlackboard.Net;

/// <summary>
/// Interface that defines methods for a type-safe blackboard system
/// to store, retrieve, manage, and remove key-value pairs.
/// </summary>
public interface IBlackboard
{
    /// <summary>
    /// Sets a value in the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">The value to store in the blackboard.</param>
    /// <returns>True if the value was successfully set; otherwise, false.</returns>
    bool SetValue<T>(string key, T value);

    /// <summary>
    /// Retrieves a value from the blackboard with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <returns>The value associated with the specified key.</returns>
    T? GetValue<T>(string key);

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
    bool TryGetValue<T>(string key, out T? value);

    /// <summary>
    /// Checks if the blackboard contains a value with the specified key of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value to check.</typeparam>
    /// <param name="key">The key to check for existence in the blackboard.</param>
    /// <returns>True if a value of the specified type exists with the given key; otherwise, false.</returns>
    bool HasValue<T>(string key);

    /// <summary>
    /// Removes all values from the blackboard, resetting it to an empty state.
    /// </summary>
    void ClearBlackboard();

    /// <summary>
    /// Removes a value from the blackboard associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to remove.</typeparam>
    /// <param name="key">The key associated with the value to be removed.</param>
    /// <returns>The value that was removed from the blackboard.</returns>
    T? RemoveValue<T>(string key);

    /// <summary>
    /// Attempts to remove a value with the specified key from the blackboard.
    /// </summary>
    /// <typeparam name="T">The type of the value to remove.</typeparam>
    /// <param name="key">The key associated with the value to remove.</param>
    /// <param name="value">The removed value if the key exists and the type matches; otherwise, the default value of <typeparamref name="T"/>.</param>
    /// <returns>True if the value was successfully removed; otherwise, false.</returns>
    bool TryRemoveValue<T>(string key, out T? value);
}

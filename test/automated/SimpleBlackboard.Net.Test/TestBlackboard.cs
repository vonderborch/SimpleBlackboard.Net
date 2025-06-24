using NUnit.Framework;

namespace SimpleBlackboard.Net.Test;

[TestFixture]
public class TestBlackboard
{
    private Blackboard _blackboard;
    private Blackboard _concurrentBlackboard;

    [SetUp]
    public void Setup()
    {
        _blackboard = new Blackboard(useConcurrentDictionary: false);
        _concurrentBlackboard = new Blackboard(useConcurrentDictionary: true);
    }

    #region SetValue Tests

    [Test]
    public void SetValue_StoresValueCorrectly()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;

        // Act
        bool result = _blackboard.SetValue(key, value);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_blackboard.GetValue<int>(key), Is.EqualTo(value));
    }

    [Test]
    public void SetValue_OverwritesExistingValue()
    {
        // Arrange
        const string key = "testKey";
        const int initialValue = 42;
        const int newValue = 100;
        _blackboard.SetValue(key, initialValue);

        // Act
        bool result = _blackboard.SetValue(key, newValue);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_blackboard.GetValue<int>(key), Is.EqualTo(newValue));
    }

    [Test]
    public void SetValue_CanStoreNullableType()
    {
        // Arrange
        const string key = "nullKey";
        string? value = null;

        // Act
        bool result = _blackboard.SetValue(key, value);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_blackboard.GetValue<string>(key), Is.Null);
    }

    [Test]
    public void SetValue_CanStoreDifferentTypes()
    {
        // Arrange
        const string intKey = "intKey";
        const string stringKey = "stringKey";
        const string boolKey = "boolKey";
        const int intValue = 42;
        const string stringValue = "test";
        const bool boolValue = true;

        // Act
        _blackboard.SetValue(intKey, intValue);
        _blackboard.SetValue(stringKey, stringValue);
        _blackboard.SetValue(boolKey, boolValue);

        // Assert
        Assert.That(_blackboard.GetValue<int>(intKey), Is.EqualTo(intValue));
        Assert.That(_blackboard.GetValue<string>(stringKey), Is.EqualTo(stringValue));
        Assert.That(_blackboard.GetValue<bool>(boolKey), Is.EqualTo(boolValue));
    }

    #endregion

    #region GetValue Tests

    [Test]
    public void GetValue_ReturnsCorrectValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        int result = _blackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void GetValue_ReturnsDefaultForMissingKey()
    {
        // Arrange
        const string key = "missingKey";

        // Act
        int result = _blackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void GetValue_ReturnsDefaultForTypeMismatch()
    {
        // Arrange
        const string key = "testKey";
        const string value = "string value";
        _blackboard.SetValue(key, value);

        // Act
        int result = _blackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    #endregion

    #region TryGetValue Tests

    [Test]
    public void TryGetValue_ReturnsTrueAndCorrectValueForExistingKey()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        bool success = _blackboard.TryGetValue<int>(key, out int result);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void TryGetValue_ReturnsFalseForMissingKey()
    {
        // Arrange
        const string key = "missingKey";

        // Act
        bool success = _blackboard.TryGetValue<int>(key, out int result);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void TryGetValue_ReturnsFalseForTypeMismatch()
    {
        // Arrange
        const string key = "testKey";
        const string value = "string value";
        _blackboard.SetValue(key, value);

        // Act
        bool success = _blackboard.TryGetValue<int>(key, out int result);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(int)));
    }

    #endregion

    #region HasValue Tests

    [Test]
    public void HasValue_ReturnsTrueForExistingKey()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        bool result = _blackboard.HasValue<int>(key);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void HasValue_ReturnsFalseForMissingKey()
    {
        // Arrange
        const string key = "missingKey";

        // Act
        bool result = _blackboard.HasValue<int>(key);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void HasValue_ReturnsTrueRegardlessOfType()
    {
        // Arrange
        const string key = "testKey";
        const string value = "string value";
        _blackboard.SetValue(key, value);

        // Act
        bool result = _blackboard.HasValue<object>(key);

        // Assert
        // This is testing the current implementation behavior, which doesn't check type in HasValue
        Assert.That(result, Is.True);
    }

    #endregion

    #region ClearBlackboard Tests

    [Test]
    public void ClearBlackboard_RemovesAllValues()
    {
        // Arrange
        _blackboard.SetValue("key1", 42);
        _blackboard.SetValue("key2", "value");
        _blackboard.SetValue("key3", true);

        // Act
        _blackboard.ClearBlackboard();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_blackboard.HasValue<int>("key1"), Is.False);
            Assert.That(_blackboard.HasValue<string>("key2"), Is.False);
            Assert.That(_blackboard.HasValue<bool>("key3"), Is.False);
        });
    }

    #endregion

    #region RemoveValue Tests

    [Test]
    public void RemoveValue_ReturnsValueAndRemovesIt()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        int removedValue = _blackboard.RemoveValue<int>(key);

        // Assert
        Assert.That(removedValue, Is.EqualTo(value));
        Assert.That(_blackboard.HasValue<int>(key), Is.False);
    }

    [Test]
    public void RemoveValue_ReturnsDefaultForMissingKey()
    {
        // Arrange
        const string key = "missingKey";

        // Act
        int removedValue = _blackboard.RemoveValue<int>(key);

        // Assert
        Assert.That(removedValue, Is.EqualTo(default(int)));
    }

    [Test]
    public void RemoveValue_ReturnsDefaultForTypeMismatch()
    {
        // Arrange
        const string key = "testKey";
        const string value = "string value";
        _blackboard.SetValue(key, value);

        // Act
        int removedValue = _blackboard.RemoveValue<int>(key);

        // Assert
        Assert.That(removedValue, Is.EqualTo(default(int)));
        Assert.That(_blackboard.HasValue<string>(key), Is.True); // Key still exists
    }

    #endregion

    #region TryRemoveValue Tests

    [Test]
    public void TryRemoveValue_ReturnsTrueAndRemovesValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        bool success = _blackboard.TryRemoveValue<int>(key, out int removedValue);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(removedValue, Is.EqualTo(value));
        Assert.That(_blackboard.HasValue<int>(key), Is.False);
    }

    [Test]
    public void TryRemoveValue_ReturnsFalseForMissingKey()
    {
        // Arrange
        const string key = "missingKey";

        // Act
        bool success = _blackboard.TryRemoveValue<int>(key, out int removedValue);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(removedValue, Is.EqualTo(default(int)));
    }

    [Test]
    public void TryRemoveValue_ReturnsFalseForTypeMismatch()
    {
        // Arrange
        const string key = "testKey";
        const string value = "string value";
        _blackboard.SetValue(key, value);

        // Act
        bool success = _blackboard.TryRemoveValue<int>(key, out int removedValue);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(removedValue, Is.EqualTo(default(int)));
        Assert.That(_blackboard.HasValue<string>(key), Is.True); // Key still exists
    }

    #endregion

    #region Complex Types Tests

    [Test]
    public void Blackboard_CanStoreAndRetrieveComplexTypes()
    {
        // Arrange
        const string key = "person";
        var person = new Person { Name = "John Doe", Age = 30 };

        // Act
        _blackboard.SetValue(key, person);
        var retrievedPerson = _blackboard.GetValue<Person>(key);

        // Assert
        Assert.That(retrievedPerson, Is.Not.Null);
        Assert.That(retrievedPerson.Name, Is.EqualTo(person.Name));
        Assert.That(retrievedPerson.Age, Is.EqualTo(person.Age));
    }

    private class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    #endregion

    #region Concurrent Dictionary Tests

    [Test]
    public void ConcurrentDictionary_SetValue_StoresValueCorrectly()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;

        // Act
        bool result = _concurrentBlackboard.SetValue(key, value);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_concurrentBlackboard.GetValue<int>(key), Is.EqualTo(value));
    }

    [Test]
    public void ConcurrentDictionary_GetValue_ReturnsCorrectValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _concurrentBlackboard.SetValue(key, value);

        // Act
        int result = _concurrentBlackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void ConcurrentDictionary_TryGetValue_ReturnsTrueAndCorrectValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _concurrentBlackboard.SetValue(key, value);

        // Act
        bool success = _concurrentBlackboard.TryGetValue<int>(key, out int result);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void ConcurrentDictionary_HasValue_ReturnsTrueForExistingKey()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _concurrentBlackboard.SetValue(key, value);

        // Act
        bool result = _concurrentBlackboard.HasValue<int>(key);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ConcurrentDictionary_RemoveValue_RemovesAndReturnsValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _concurrentBlackboard.SetValue(key, value);

        // Act
        int removedValue = _concurrentBlackboard.RemoveValue<int>(key);

        // Assert
        Assert.That(removedValue, Is.EqualTo(value));
        Assert.That(_concurrentBlackboard.HasValue<int>(key), Is.False);
    }

    [Test]
    public void ConcurrentDictionary_TryRemoveValue_ReturnsTrueAndRemovesValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _concurrentBlackboard.SetValue(key, value);

        // Act
        bool success = _concurrentBlackboard.TryRemoveValue<int>(key, out int removedValue);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(removedValue, Is.EqualTo(value));
        Assert.That(_concurrentBlackboard.HasValue<int>(key), Is.False);
    }

    [Test]
    public void ConcurrentDictionary_ClearBlackboard_RemovesAllValues()
    {
        // Arrange
        _concurrentBlackboard.SetValue("key1", 42);
        _concurrentBlackboard.SetValue("key2", "value");
        _concurrentBlackboard.SetValue("key3", true);

        // Act
        _concurrentBlackboard.ClearBlackboard();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_concurrentBlackboard.HasValue<int>("key1"), Is.False);
            Assert.That(_concurrentBlackboard.HasValue<string>("key2"), Is.False);
            Assert.That(_concurrentBlackboard.HasValue<bool>("key3"), Is.False);
        });
    }

    [Test]
    public void ConcurrentDictionary_CanStoreAndRetrieveComplexTypes()
    {
        // Arrange
        const string key = "person";
        var person = new Person { Name = "John Doe", Age = 30 };

        // Act
        _concurrentBlackboard.SetValue(key, person);
        var retrievedPerson = _concurrentBlackboard.GetValue<Person>(key);

        // Assert
        Assert.That(retrievedPerson, Is.Not.Null);
        Assert.That(retrievedPerson.Name, Is.EqualTo(person.Name));
        Assert.That(retrievedPerson.Age, Is.EqualTo(person.Age));
    }

    #endregion
}

using NUnit.Framework;

namespace SimpleBlackboard.Net.Test;

[TestFixture]
public class TestBlackboard
{
    private class TestableBlackboard : Blackboard
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

    private Blackboard _blackboard = null!;

    [SetUp]
    public void Setup()
    {
        _blackboard = new TestableBlackboard();
    }

    [Test]
    public void SetValue_StoresValue()
    {
        // Arrange
        const string key = "testKey";
        const string value = "testValue";

        // Act
        _blackboard.SetValue(key, value);

        // Assert
        Assert.That(_blackboard.GetValue<string>(key), Is.EqualTo(value));
    }

    [Test]
    public void GetValue_KeyExists_ReturnsValue()
    {
        // Arrange
        const string key = "testKey";
        const int value = 42;
        _blackboard.SetValue(key, value);

        // Act
        var result = _blackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void GetValue_KeyDoesNotExist_ReturnsDefault()
    {
        // Arrange
        const string key = "nonExistentKey";

        // Act
        var result = _blackboard.GetValue<string>(key);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetValue_WrongType_ReturnsDefault()
    {
        // Arrange
        const string key = "testKey";
        _blackboard.SetValue(key, "string value");

        // Act
        var result = _blackboard.GetValue<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void SetValue_UpdatesExistingKey()
    {
        // Arrange
        const string key = "testKey";
        _blackboard.SetValue(key, "original value");

        // Act
        _blackboard.SetValue(key, "updated value");

        // Assert
        Assert.That(_blackboard.GetValue<string>(key), Is.EqualTo("updated value"));
    }

    [Test]
    public void GetValue_MultipleTypes_StoredSeparately()
    {
        // Arrange
        const string key1 = "intKey";
        const string key2 = "stringKey";
        const string key3 = "boolKey";

        // Act
        _blackboard.SetValue(key1, 42);
        _blackboard.SetValue(key2, "hello");
        _blackboard.SetValue(key3, true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_blackboard.GetValue<int>(key1), Is.EqualTo(42));
            Assert.That(_blackboard.GetValue<string>(key2), Is.EqualTo("hello"));
            Assert.That(_blackboard.GetValue<bool>(key3), Is.EqualTo(true));
        });
    }
    
    [Test]
    public void ClearBlackboard_RemovesAllValues()
    {
        // Arrange
        _blackboard.SetValue("key1", "value1");
        _blackboard.SetValue("key2", 123);

        // Act
        _blackboard.ClearBlackboard();

        // Assert
        Assert.That(_blackboard.GetValue<string>("key1"), Is.Null);
        Assert.That(_blackboard.GetValue<int>("key2"), Is.EqualTo(default(int)));
    }

    [Test]
    public void HasValue_KeyExists_ReturnsTrue()
    {
        // Arrange
        const string key = "testKey";
        _blackboard.SetValue(key, "testValue");
        
        // Act
        var result = _blackboard.HasValue<string>(key);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void HasValue_KeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        const string key = "nonExistentKey";

        // Act
        var result = _blackboard.HasValue<string>(key);

        // Assert
        Assert.That(result, Is.False);
    }
}

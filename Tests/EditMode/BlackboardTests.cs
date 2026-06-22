using System;
using System.Collections.Generic;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class BlackboardTests
    {
        private Blackboard _blackboard;

        [SetUp]
        public void SetUp()
        {
            _blackboard = new Blackboard();
        }

        [Test]
        public void SetValue_And_GetValue_Int()
        {
            _blackboard.SetValue("hp", 100);
            Assert.AreEqual(100, _blackboard.GetValue<int>("hp"));
        }

        [Test]
        public void SetValue_And_GetValue_String()
        {
            _blackboard.SetValue("name", "player1");
            Assert.AreEqual("player1", _blackboard.GetValue<string>("name"));
        }

        [Test]
        public void SetValue_And_GetValue_Float()
        {
            _blackboard.SetValue("speed", 3.5f);
            Assert.AreEqual(3.5f, _blackboard.GetValue<float>("speed"));
        }

        [Test]
        public void SetValue_And_GetValue_Bool()
        {
            _blackboard.SetValue("alive", true);
            Assert.AreEqual(true, _blackboard.GetValue<bool>("alive"));
        }

        [Test]
        public void GetValue_NonExistentKey_ReturnsDefault()
        {
            Assert.AreEqual(0, _blackboard.GetValue<int>("missing"));
            Assert.IsNull(_blackboard.GetValue<string>("missing"));
            Assert.AreEqual(false, _blackboard.GetValue<bool>("missing"));
        }

        [Test]
        public void GetValue_NonExistentKey_ReturnsCustomDefault()
        {
            Assert.AreEqual(42, _blackboard.GetValue("missing", 42));
            Assert.AreEqual("fallback", _blackboard.GetValue("missing", "fallback"));
        }

        [Test]
        public void SetValue_OverwriteExistingValue()
        {
            _blackboard.SetValue("hp", 100);
            _blackboard.SetValue("hp", 200);
            Assert.AreEqual(200, _blackboard.GetValue<int>("hp"));
        }

        [Test]
        public void SetValue_OverwriteWithDifferentType()
        {
            _blackboard.SetValue("val", 42);
            _blackboard.SetValue("val", "now a string");
            Assert.AreEqual("now a string", _blackboard.GetValue<string>("val"));
        }

        [Test]
        public void GetValue_WrongType_ReturnsDefault()
        {
            _blackboard.SetValue("hp", 100);
            Assert.AreEqual(0f, _blackboard.GetValue<float>("hp"));
        }

        [Test]
        public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
        {
            _blackboard.SetValue("hp", 100);
            bool found = _blackboard.TryGetValue<int>("hp", out int val);
            Assert.IsTrue(found);
            Assert.AreEqual(100, val);
        }

        [Test]
        public void TryGetValue_NonExistentKey_ReturnsFalse()
        {
            bool found = _blackboard.TryGetValue<int>("missing", out int val);
            Assert.IsFalse(found);
            Assert.AreEqual(0, val);
        }

        [Test]
        public void TryGetValue_WrongType_ReturnsFalse()
        {
            _blackboard.SetValue("hp", 100);
            bool found = _blackboard.TryGetValue<string>("hp", out string val);
            Assert.IsFalse(found);
            Assert.IsNull(val);
        }

        [Test]
        public void HasValue_ExistingKey_ReturnsTrue()
        {
            _blackboard.SetValue("hp", 100);
            Assert.IsTrue(_blackboard.HasValue("hp"));
        }

        [Test]
        public void HasValue_NonExistentKey_ReturnsFalse()
        {
            Assert.IsFalse(_blackboard.HasValue("missing"));
        }

        [Test]
        public void HasValue_TypedGeneric_MatchingType()
        {
            _blackboard.SetValue("hp", 100);
            Assert.IsTrue(_blackboard.HasValue<int>("hp"));
            Assert.IsFalse(_blackboard.HasValue<string>("hp"));
        }

        [Test]
        public void RemoveValue_ExistingKey()
        {
            _blackboard.SetValue("hp", 100);
            _blackboard.RemoveValue("hp");
            Assert.IsFalse(_blackboard.HasValue("hp"));
        }

        [Test]
        public void RemoveValue_NonExistentKey_NoException()
        {
            Assert.DoesNotThrow(() => _blackboard.RemoveValue("missing"));
        }

        [Test]
        public void Clear_RemovesAllValues()
        {
            _blackboard.SetValue("hp", 100);
            _blackboard.SetValue("name", "player");
            _blackboard.SetValue("alive", true);
            _blackboard.Clear();
            Assert.IsFalse(_blackboard.HasValue("hp"));
            Assert.IsFalse(_blackboard.HasValue("name"));
            Assert.IsFalse(_blackboard.HasValue("alive"));
        }

        [Test]
        public void GetAllKeys_ReturnsAllKeys()
        {
            _blackboard.SetValue("a", 1);
            _blackboard.SetValue("b", 2);
            _blackboard.SetValue("c", 3);
            List<string> keys = _blackboard.GetAllKeys();
            Assert.AreEqual(3, keys.Count);
            Assert.Contains("a", keys);
            Assert.Contains("b", keys);
            Assert.Contains("c", keys);
        }

        [Test]
        public void GetAllKeys_EmptyBlackboard_ReturnsEmptyList()
        {
            List<string> keys = _blackboard.GetAllKeys();
            Assert.AreEqual(0, keys.Count);
        }

        [Test]
        public void GetValueType_ExistingKey_ReturnsCorrectType()
        {
            _blackboard.SetValue("hp", 100);
            _blackboard.SetValue("name", "player");
            Assert.AreEqual(typeof(int), _blackboard.GetValueType("hp"));
            Assert.AreEqual(typeof(string), _blackboard.GetValueType("name"));
        }

        [Test]
        public void GetValueType_NonExistentKey_ReturnsNull()
        {
            Assert.IsNull(_blackboard.GetValueType("missing"));
        }

        [Test]
        public void RegisterValueChanged_CallbackInvokedOnSet()
        {
            int receivedValue = 0;
            string receivedKey = null;
            _blackboard.RegisterValueChanged<int>((key, value) =>
            {
                receivedKey = key;
                receivedValue = value;
            });
            _blackboard.SetValue("hp", 42);
            Assert.AreEqual("hp", receivedKey);
            Assert.AreEqual(42, receivedValue);
        }

        [Test]
        public void RegisterValueChanged_CallbackInvokedOnUpdate()
        {
            int callCount = 0;
            _blackboard.RegisterValueChanged<int>((key, value) => callCount++);
            _blackboard.SetValue("hp", 100);
            _blackboard.SetValue("hp", 200);
            Assert.AreEqual(2, callCount);
        }

        [Test]
        public void UnregisterValueChanged_CallbackNoLongerInvoked()
        {
            int callCount = 0;
            Action<string, int> callback = (key, value) => callCount++;
            _blackboard.RegisterValueChanged(callback);
            _blackboard.SetValue("hp", 100);
            Assert.AreEqual(1, callCount);

            _blackboard.UnregisterValueChanged(callback);
            _blackboard.SetValue("hp", 200);
            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void RegisterValueRemoved_CallbackInvokedOnRemove()
        {
            string removedKey = null;
            _blackboard.RegisterValueRemoved(key => removedKey = key);
            _blackboard.SetValue("hp", 100);
            _blackboard.RemoveValue("hp");
            Assert.AreEqual("hp", removedKey);
        }

        [Test]
        public void RegisterValueRemoved_NotCalledForNonExistentKey()
        {
            int callCount = 0;
            _blackboard.RegisterValueRemoved(key => callCount++);
            _blackboard.RemoveValue("missing");
            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void UnregisterValueRemoved_CallbackNoLongerInvoked()
        {
            int callCount = 0;
            Action<string> callback = key => callCount++;
            _blackboard.RegisterValueRemoved(callback);
            _blackboard.SetValue("hp", 100);
            _blackboard.RemoveValue("hp");
            Assert.AreEqual(1, callCount);

            _blackboard.UnregisterValueRemoved(callback);
            _blackboard.SetValue("hp", 200);
            _blackboard.RemoveValue("hp");
            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void SetValue_ComplexType()
        {
            var list = new List<int> { 1, 2, 3 };
            _blackboard.SetValue("items", list);
            var result = _blackboard.GetValue<List<int>>("items");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0]);
        }

        [Test]
        public void MultipleTypes_CoexistCorrectly()
        {
            _blackboard.SetValue("intVal", 42);
            _blackboard.SetValue("strVal", "hello");
            _blackboard.SetValue("boolVal", true);
            _blackboard.SetValue("floatVal", 1.5f);

            Assert.AreEqual(42, _blackboard.GetValue<int>("intVal"));
            Assert.AreEqual("hello", _blackboard.GetValue<string>("strVal"));
            Assert.AreEqual(true, _blackboard.GetValue<bool>("boolVal"));
            Assert.AreEqual(1.5f, _blackboard.GetValue<float>("floatVal"));
        }
    }
}

using System;
using System.Collections.Generic;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class IDictionaryExtsTests
    {
        [Test]
        public void Invert_SwapsKeysAndValues()
        {
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var inverted = dict.Invert();
            Assert.AreEqual("a", inverted[1]);
            Assert.AreEqual("b", inverted[2]);
        }

        [Test]
        public void TryAdd_NewKey_ReturnsTrueAndAdds()
        {
            var dict = new Dictionary<string, int> { { "a", 1 } };
            bool result = dict.TryAdd("b", 2);
            Assert.IsTrue(result);
            Assert.AreEqual(2, dict["b"]);
        }

        [Test]
        public void TryAdd_ExistingKey_ReturnsFalse()
        {
            var dict = new Dictionary<string, int> { { "a", 1 } };
            bool result = dict.TryAdd("a", 99);
            Assert.IsFalse(result);
            Assert.AreEqual(1, dict["a"]);
        }

        [Test]
        public void Remove_ExistingKey_ReturnsTrueAndOutputsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            bool result = dict.Remove("a", out int value);
            Assert.IsTrue(result);
            Assert.AreEqual(1, value);
            Assert.IsFalse(dict.ContainsKey("a"));
        }

        [Test]
        public void Remove_NonExistentKey_ReturnsFalse()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 1 } };
            bool result = dict.Remove("z", out int value);
            Assert.IsFalse(result);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void TryRemove_ExistingKey_ReturnsTrueAndOutputsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "x", 42 } };
            bool result = dict.TryRemove("x", out int value);
            Assert.IsTrue(result);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void TryRemove_NonExistentKey_ReturnsFalse()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            bool result = dict.TryRemove("x", out int value);
            Assert.IsFalse(result);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void AddOrUpdate_NewKey_AddsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            dict.AddOrUpdate("a", k => 10, (k, old) => old + 1);
            Assert.AreEqual(10, dict["a"]);
        }

        [Test]
        public void AddOrUpdate_ExistingKey_UpdatesValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 10 } };
            dict.AddOrUpdate("a", k => 0, (k, old) => old + 5);
            Assert.AreEqual(15, dict["a"]);
        }

        [Test]
        public void GetOrDefault_ExistingKey_ReturnsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 42 } };
            Assert.AreEqual(42, dict.GetOrDefault("a"));
        }

        [Test]
        public void GetOrDefault_NonExistentKey_ReturnsDefault()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            Assert.AreEqual(0, dict.GetOrDefault("a"));
            Assert.AreEqual(99, dict.GetOrDefault("a", 99));
        }

        [Test]
        public void GetOrDefault_WithFactory_NonExistentKey_ReturnsFactoryValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            Assert.AreEqual(42, dict.GetOrDefault("a", () => 42));
        }

        [Test]
        public void GetOrDefault_Nullable_ExistingKey_ReturnsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 10 } };
            int? result = dict.GetOrDefault("a", (int?)null);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetOrDefault_Nullable_NonExistentKey_ReturnsNull()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            int? result = dict.GetOrDefault("a", (int?)null);
            Assert.IsNull(result);
        }

        [Test]
        public void GetOrAdd_NewKey_AddsAndReturns()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            int result = dict.GetOrAdd("a", () => 42);
            Assert.AreEqual(42, result);
            Assert.AreEqual(42, dict["a"]);
        }

        [Test]
        public void GetOrAdd_ExistingKey_ReturnsExisting()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 10 } };
            int result = dict.GetOrAdd("a", () => 42);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void GetOrAdd_NewConstraint_NewKey_AddsDefault()
        {
            IDictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
            var result = dict.GetOrAdd<string, List<int>>("a");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsTrue(dict.ContainsKey("a"));
        }

        [Test]
        public void GetAndIncrement_NewKey_SetsStartValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            int result = dict.GetAndIncrement("counter", 0);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetAndIncrement_ExistingKey_Increments()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "counter", 5 } };
            int result = dict.GetAndIncrement("counter");
            Assert.AreEqual(6, result);
            Assert.AreEqual(6, dict["counter"]);
        }

        [Test]
        public void GetValue_ExistingKey_ReturnsValue()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 42 } };
            Assert.AreEqual(42, dict.GetValue("a"));
        }

        [Test]
        public void GetValue_NonExistentKey_ReturnsDefault()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            Assert.AreEqual(0, dict.GetValue("missing"));
        }

        [Test]
        public void ForEach_IteratesAllPairs()
        {
            IDictionary<string, int> dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            int sum = 0;
            dict.ForEach((k, v) => sum += v);
            Assert.AreEqual(3, sum);
        }

        [Test]
        public void ForEach_EmptyDictionary_NoIteration()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            int count = 0;
            dict.ForEach((k, v) => count++);
            Assert.AreEqual(0, count);
        }
    }
}

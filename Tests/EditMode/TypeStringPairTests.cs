using System;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class TypeStringPairTests
    {
        [Test]
        public void Constructor_WithTypeOnly_SetsTypeAndEmptyString()
        {
            var pair = new TypeStringPair(typeof(int));
            Assert.AreEqual(typeof(int), pair.Type);
            Assert.AreEqual("", pair.String);
        }

        [Test]
        public void Constructor_WithTypeAndString_SetsBoth()
        {
            var pair = new TypeStringPair(typeof(string), "test");
            Assert.AreEqual(typeof(string), pair.Type);
            Assert.AreEqual("test", pair.String);
        }

        [Test]
        public void Equals_SameTypeAndString_ReturnsTrue()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(int), "a");
            Assert.IsTrue(pair1.Equals(pair2));
        }

        [Test]
        public void Equals_DifferentType_ReturnsFalse()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(string), "a");
            Assert.IsFalse(pair1.Equals(pair2));
        }

        [Test]
        public void Equals_DifferentString_ReturnsFalse()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(int), "b");
            Assert.IsFalse(pair1.Equals(pair2));
        }

        [Test]
        public void OperatorEquals_SamePairs_ReturnsTrue()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(int), "a");
            Assert.IsTrue(pair1 == pair2);
        }

        [Test]
        public void OperatorNotEquals_DifferentPairs_ReturnsTrue()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(string), "b");
            Assert.IsTrue(pair1 != pair2);
        }

        [Test]
        public void Equals_Object_SameValue_ReturnsTrue()
        {
            var pair1 = new TypeStringPair(typeof(int), "x");
            object pair2 = new TypeStringPair(typeof(int), "x");
            Assert.IsTrue(pair1.Equals(pair2));
        }

        [Test]
        public void Equals_Object_DifferentType_ReturnsFalse()
        {
            var pair = new TypeStringPair(typeof(int), "x");
            Assert.IsFalse(pair.Equals("not a pair"));
        }

        [Test]
        public void GetHashCode_SamePairs_SameHash()
        {
            var pair1 = new TypeStringPair(typeof(int), "test");
            var pair2 = new TypeStringPair(typeof(int), "test");
            Assert.AreEqual(pair1.GetHashCode(), pair2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentPairs_DifferentHash()
        {
            var pair1 = new TypeStringPair(typeof(int), "a");
            var pair2 = new TypeStringPair(typeof(string), "b");
            // While hash collisions are possible, these should differ
            Assert.AreNotEqual(pair1.GetHashCode(), pair2.GetHashCode());
        }

        [Test]
        public void ToString_TypeOnly_ReturnsFullTypeName()
        {
            var pair = new TypeStringPair(typeof(int));
            Assert.AreEqual("System.Int32", pair.ToString());
        }

        [Test]
        public void ToString_TypeAndString_ReturnsTypeDotString()
        {
            var pair = new TypeStringPair(typeof(int), "myField");
            Assert.AreEqual("System.Int32.myField", pair.ToString());
        }

        [Test]
        public void ToString_NullType_ThrowsArgumentNullException()
        {
            var pair = new TypeStringPair(null, "test");
            Assert.Throws<ArgumentNullException>(() => pair.ToString());
        }

        [Test]
        public void Clear_ResetsTypeAndString()
        {
            var pair = new TypeStringPair(typeof(int), "test");
            pair.Clear();
            Assert.IsNull(pair.Type);
            Assert.IsNull(pair.String);
        }
    }
}

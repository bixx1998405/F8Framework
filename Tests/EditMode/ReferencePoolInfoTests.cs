using System;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class ReferencePoolInfoTests
    {
        [Test]
        public void Constructor_SetsAllProperties()
        {
            var info = new ReferencePoolInfo(typeof(int), 10, 5, 15, 8, 20, 3);
            Assert.AreEqual(typeof(int), info.Type);
            Assert.AreEqual(10, info.UnusedReferenceCount);
            Assert.AreEqual(5, info.UsingReferenceCount);
            Assert.AreEqual(15, info.AcquireReferenceCount);
            Assert.AreEqual(8, info.ReleaseReferenceCount);
            Assert.AreEqual(20, info.AddReferenceCount);
            Assert.AreEqual(3, info.RemoveReferenceCount);
        }

        [Test]
        public void Constructor_WithStringType()
        {
            var info = new ReferencePoolInfo(typeof(string), 0, 0, 0, 0, 0, 0);
            Assert.AreEqual(typeof(string), info.Type);
        }

        [Test]
        public void Constructor_ZeroCounts()
        {
            var info = new ReferencePoolInfo(typeof(object), 0, 0, 0, 0, 0, 0);
            Assert.AreEqual(0, info.UnusedReferenceCount);
            Assert.AreEqual(0, info.UsingReferenceCount);
            Assert.AreEqual(0, info.AcquireReferenceCount);
            Assert.AreEqual(0, info.ReleaseReferenceCount);
            Assert.AreEqual(0, info.AddReferenceCount);
            Assert.AreEqual(0, info.RemoveReferenceCount);
        }

        [Test]
        public void Constructor_LargeNumbers()
        {
            var info = new ReferencePoolInfo(typeof(int), int.MaxValue, int.MaxValue,
                int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            Assert.AreEqual(int.MaxValue, info.UnusedReferenceCount);
            Assert.AreEqual(int.MaxValue, info.UsingReferenceCount);
        }

        [Test]
        public void DefaultStruct_HasDefaultValues()
        {
            var info = default(ReferencePoolInfo);
            Assert.IsNull(info.Type);
            Assert.AreEqual(0, info.UnusedReferenceCount);
            Assert.AreEqual(0, info.UsingReferenceCount);
            Assert.AreEqual(0, info.AcquireReferenceCount);
            Assert.AreEqual(0, info.ReleaseReferenceCount);
            Assert.AreEqual(0, info.AddReferenceCount);
            Assert.AreEqual(0, info.RemoveReferenceCount);
        }

        [Test]
        public void Type_Property_ReturnsCorrectType()
        {
            var info = new ReferencePoolInfo(typeof(DateTime), 1, 2, 3, 4, 5, 6);
            Assert.AreEqual(typeof(DateTime), info.Type);
        }

        [Test]
        public void MultipleDifferentInfos_AreIndependent()
        {
            var info1 = new ReferencePoolInfo(typeof(int), 10, 5, 15, 8, 20, 3);
            var info2 = new ReferencePoolInfo(typeof(string), 1, 2, 3, 4, 5, 6);

            Assert.AreEqual(typeof(int), info1.Type);
            Assert.AreEqual(10, info1.UnusedReferenceCount);
            Assert.AreEqual(typeof(string), info2.Type);
            Assert.AreEqual(1, info2.UnusedReferenceCount);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class ListExtsTests
    {
        [Test]
        public void RemoveLast_RemovesAndReturnsLastElement()
        {
            var list = new List<int> { 1, 2, 3 };
            int result = list.RemoveLast();
            Assert.AreEqual(3, result);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[list.Count - 1]);
        }

        [Test]
        public void RemoveLast_EmptyList_ReturnsDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.RemoveLast());
        }

        [Test]
        public void RemoveLast_NullList_ReturnsDefault()
        {
            List<string> list = null;
            Assert.IsNull(list.RemoveLast());
        }

        [Test]
        public void RemoveFirst_RemovesAndReturnsFirstElement()
        {
            var list = new List<int> { 1, 2, 3 };
            int result = list.RemoveFirst();
            Assert.AreEqual(1, result);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[0]);
        }

        [Test]
        public void RemoveFirst_EmptyList_ReturnsDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.RemoveFirst());
        }

        [Test]
        public void RemoveFirst_NullList_ReturnsDefault()
        {
            List<string> list = null;
            Assert.IsNull(list.RemoveFirst());
        }

        [Test]
        public void First_ReturnsFirstElement()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(10, list.First());
        }

        [Test]
        public void First_EmptyList_ReturnsDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.First());
        }

        [Test]
        public void First_NullList_ReturnsDefault()
        {
            List<string> list = null;
            Assert.IsNull(list.First());
        }

        [Test]
        public void Last_ReturnsLastElement()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(30, list.Last());
        }

        [Test]
        public void Last_EmptyList_ReturnsDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.Last());
        }

        [Test]
        public void Last_NullList_ReturnsDefault()
        {
            List<string> list = null;
            Assert.IsNull(list.Last());
        }

        [Test]
        public void InsertRange_InsertsAtSpecifiedIndex()
        {
            var list = new List<int> { 1, 4, 5 };
            list.InsertRange(1, new[] { 2, 3 });
            Assert.AreEqual(new List<int> { 1, 2, 3, 4, 5 }, list);
        }

        [Test]
        public void InsertRange_AtStart()
        {
            var list = new List<int> { 3, 4 };
            list.InsertRange(0, new[] { 1, 2 });
            Assert.AreEqual(new List<int> { 1, 2, 3, 4 }, list);
        }

        [Test]
        public void AtWrapped_NormalIndex()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(20, list.AtWrapped(1));
        }

        [Test]
        public void AtWrapped_IndexBeyondCount_Wraps()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(10, list.AtWrapped(3));
            Assert.AreEqual(20, list.AtWrapped(4));
        }

        [Test]
        public void AtWrapped_NegativeIndex_Wraps()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(30, list.AtWrapped(-1));
            Assert.AreEqual(20, list.AtWrapped(-2));
        }

        [Test]
        public void AtWrapped_EmptyList_Throws()
        {
            var list = new List<int>();
            Assert.Throws<IndexOutOfRangeException>(() => list.AtWrapped(0));
        }

        [Test]
        public void AtWrappedOrDefault_EmptyList_ReturnsDefault()
        {
            var list = new List<int>();
            Assert.AreEqual(0, list.AtWrappedOrDefault(0));
            Assert.AreEqual(42, list.AtWrappedOrDefault(0, 42));
        }

        [Test]
        public void AtWrappedOrDefault_NonEmptyList_ReturnsWrapped()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(10, list.AtWrappedOrDefault(3));
        }

        [Test]
        public void SetAtWrapped_SetsValueAtWrappedIndex()
        {
            var list = new List<int> { 10, 20, 30 };
            list.SetAtWrapped(3, 99);
            Assert.AreEqual(99, list[0]);
        }

        [Test]
        public void IndexOfOrDefault_Found_ReturnsIndex()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(1, list.IndexOfOrDefault(20, -1));
        }

        [Test]
        public void IndexOfOrDefault_NotFound_ReturnsDefaultIndex()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.AreEqual(-1, list.IndexOfOrDefault(99, -1));
        }

        [Test]
        public void ClearAndDispose_DisposesAllAndClears()
        {
            int disposeCount = 0;
            var list = new List<DisposableItem>
            {
                new DisposableItem(() => disposeCount++),
                new DisposableItem(() => disposeCount++),
            };
            list.ClearAndDispose();
            Assert.AreEqual(2, disposeCount);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void AddRangeUntyped_AddsItems()
        {
            IList list = new ArrayList();
            list.AddRangeUntyped(new object[] { 1, "two", 3.0 });
            Assert.AreEqual(3, list.Count);
        }

        [Test]
        public void RemoveRangeUntyped_RemovesItems()
        {
            IList list = new ArrayList { 1, 2, 3 };
            list.RemoveRangeUntyped(new object[] { 2 });
            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void ReplaceUntyped_ClearsAndAdds()
        {
            IList list = new ArrayList { 1, 2, 3 };
            list.ReplaceUntyped(new object[] { 4, 5 });
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(4, list[0]);
            Assert.AreEqual(5, list[1]);
        }

        [Test]
        public void ChangeIndex_MovesItemToNewIndex()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            list.ChangeIndex(3, 0);
            Assert.AreEqual(3, list[0]);
        }

        [Test]
        public void ChangeIndex_NullItem_ThrowsArgumentNullException()
        {
            var list = new List<string> { "a", "b" };
            Assert.Throws<ArgumentNullException>(() => list.ChangeIndex(null, 0));
        }

        [Test]
        public void ChangeIndex_WithPredicate_MovesItem()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            list.ChangeIndex(x => x == 4, 1);
            Assert.AreEqual(4, list[1]);
        }

        private class DisposableItem : IDisposable
        {
            private readonly Action _onDispose;
            public DisposableItem(Action onDispose) => _onDispose = onDispose;
            public void Dispose() => _onDispose();
        }
    }
}

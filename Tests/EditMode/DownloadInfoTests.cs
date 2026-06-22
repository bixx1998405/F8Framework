using System;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class DownloadInfoTests
    {
        [Test]
        public void Constructor_SetsAllProperties()
        {
            var timeSpan = TimeSpan.FromSeconds(10);
            var info = new DownloadInfo(1, "http://example.com/file.zip", "/tmp/file.zip", 1024, 0.5f, timeSpan);

            Assert.AreEqual(1, info.DownloadId);
            Assert.AreEqual("http://example.com/file.zip", info.DownloadUrl);
            Assert.AreEqual("/tmp/file.zip", info.DownloadPath);
            Assert.AreEqual(1024UL, info.DownloadedLength);
            Assert.AreEqual(0.5f, info.DownloadProgress);
            Assert.AreEqual(timeSpan, info.DownloadTimeSpan);
        }

        [Test]
        public void Equals_SameUrlAndPath_ReturnsTrue()
        {
            var info1 = new DownloadInfo(1, "http://example.com/a", "/tmp/a", 100, 0.5f, TimeSpan.Zero);
            var info2 = new DownloadInfo(2, "http://example.com/a", "/tmp/a", 200, 1.0f, TimeSpan.FromSeconds(5));
            Assert.IsTrue(info1.Equals(info2));
        }

        [Test]
        public void Equals_DifferentUrl_ReturnsFalse()
        {
            var info1 = new DownloadInfo(1, "http://example.com/a", "/tmp/a", 100, 0.5f, TimeSpan.Zero);
            var info2 = new DownloadInfo(1, "http://example.com/b", "/tmp/a", 100, 0.5f, TimeSpan.Zero);
            Assert.IsFalse(info1.Equals(info2));
        }

        [Test]
        public void Equals_DifferentPath_ReturnsFalse()
        {
            var info1 = new DownloadInfo(1, "http://example.com/a", "/tmp/a", 100, 0.5f, TimeSpan.Zero);
            var info2 = new DownloadInfo(1, "http://example.com/a", "/tmp/b", 100, 0.5f, TimeSpan.Zero);
            Assert.IsFalse(info1.Equals(info2));
        }

        [Test]
        public void ToString_ContainsAllFields()
        {
            var info = new DownloadInfo(42, "http://example.com/file.zip", "/tmp/file.zip", 2048, 0.75f, TimeSpan.FromSeconds(30));
            string str = info.ToString();
            Assert.IsTrue(str.Contains("42"));
            Assert.IsTrue(str.Contains("http://example.com/file.zip"));
            Assert.IsTrue(str.Contains("/tmp/file.zip"));
            Assert.IsTrue(str.Contains("2048"));
            Assert.IsTrue(str.Contains("0.75"));
        }

        [Test]
        public void DefaultStruct_HasDefaultValues()
        {
            var info = default(DownloadInfo);
            Assert.AreEqual(0, info.DownloadId);
            Assert.IsNull(info.DownloadUrl);
            Assert.IsNull(info.DownloadPath);
            Assert.AreEqual(0UL, info.DownloadedLength);
            Assert.AreEqual(0f, info.DownloadProgress);
            Assert.AreEqual(TimeSpan.Zero, info.DownloadTimeSpan);
        }

        [Test]
        public void Constructor_ZeroValues()
        {
            var info = new DownloadInfo(0, "", "", 0, 0f, TimeSpan.Zero);
            Assert.AreEqual(0, info.DownloadId);
            Assert.AreEqual("", info.DownloadUrl);
            Assert.AreEqual("", info.DownloadPath);
            Assert.AreEqual(0UL, info.DownloadedLength);
            Assert.AreEqual(0f, info.DownloadProgress);
        }

        [Test]
        public void Constructor_LargeValues()
        {
            var info = new DownloadInfo(long.MaxValue, "http://example.com", "/path",
                ulong.MaxValue, 1.0f, TimeSpan.MaxValue);
            Assert.AreEqual(long.MaxValue, info.DownloadId);
            Assert.AreEqual(ulong.MaxValue, info.DownloadedLength);
            Assert.AreEqual(1.0f, info.DownloadProgress);
        }
    }
}

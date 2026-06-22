using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class GameConfigTests
    {
        [Test]
        public void CompareVersions_EqualVersions_ReturnsZero()
        {
            Assert.AreEqual(0, GameConfig.CompareVersions("1.0.0", "1.0.0"));
        }

        [Test]
        public void CompareVersions_Version1Greater_ReturnsOne()
        {
            Assert.AreEqual(1, GameConfig.CompareVersions("2.0.0", "1.0.0"));
        }

        [Test]
        public void CompareVersions_Version2Greater_ReturnsNegativeOne()
        {
            Assert.AreEqual(-1, GameConfig.CompareVersions("1.0.0", "2.0.0"));
        }

        [Test]
        public void CompareVersions_MinorVersionDifference()
        {
            Assert.AreEqual(-1, GameConfig.CompareVersions("1.0.0", "1.1.0"));
            Assert.AreEqual(1, GameConfig.CompareVersions("1.2.0", "1.1.0"));
        }

        [Test]
        public void CompareVersions_PatchVersionDifference()
        {
            Assert.AreEqual(-1, GameConfig.CompareVersions("1.0.0", "1.0.1"));
            Assert.AreEqual(1, GameConfig.CompareVersions("1.0.2", "1.0.1"));
        }

        [Test]
        public void CompareVersions_DifferentLengths_ShorterTreatedAsZero()
        {
            Assert.AreEqual(0, GameConfig.CompareVersions("1.0", "1.0.0"));
            Assert.AreEqual(-1, GameConfig.CompareVersions("1.0", "1.0.1"));
            Assert.AreEqual(1, GameConfig.CompareVersions("1.0.1", "1.0"));
        }

        [Test]
        public void CompareVersions_SingleComponent()
        {
            Assert.AreEqual(0, GameConfig.CompareVersions("1", "1"));
            Assert.AreEqual(1, GameConfig.CompareVersions("2", "1"));
            Assert.AreEqual(-1, GameConfig.CompareVersions("1", "2"));
        }

        [Test]
        public void CompareVersions_MultipleComponents()
        {
            Assert.AreEqual(0, GameConfig.CompareVersions("1.2.3.4", "1.2.3.4"));
            Assert.AreEqual(1, GameConfig.CompareVersions("1.2.3.5", "1.2.3.4"));
            Assert.AreEqual(-1, GameConfig.CompareVersions("1.2.3.4", "1.2.3.5"));
        }

        [Test]
        public void CompareVersions_LargeNumbers()
        {
            Assert.AreEqual(1, GameConfig.CompareVersions("10.20.30", "10.20.29"));
            Assert.AreEqual(-1, GameConfig.CompareVersions("10.20.30", "10.20.31"));
        }

        [Test]
        public void CompareVersions_ZeroVersions()
        {
            Assert.AreEqual(0, GameConfig.CompareVersions("0.0.0", "0.0.0"));
            Assert.AreEqual(1, GameConfig.CompareVersions("0.0.1", "0.0.0"));
        }

        [Test]
        public void GameVersion_Constructor_SetsAllFields()
        {
            var gv = new GameVersion("1.0.0", "https://example.com", true, null, false, null, null);
            Assert.AreEqual("1.0.0", gv.Version);
            Assert.AreEqual("https://example.com", gv.AssetRemoteAddress);
            Assert.IsTrue(gv.EnableHotUpdate);
            Assert.IsFalse(gv.EnablePackage);
        }

        [Test]
        public void GameVersion_DefaultConstructor()
        {
            var gv = new GameVersion();
            Assert.IsNull(gv.Version);
            Assert.IsNull(gv.AssetRemoteAddress);
            Assert.IsFalse(gv.EnableHotUpdate);
            Assert.IsFalse(gv.EnablePackage);
        }
    }
}

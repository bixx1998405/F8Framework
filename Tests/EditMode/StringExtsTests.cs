using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class StringExtsTests
    {
        [Test]
        public void Contains_CaseInsensitive_MatchFound()
        {
            Assert.IsTrue("Hello World".Contains("hello", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Contains_CaseSensitive_NoMatch()
        {
            Assert.IsFalse("Hello World".Contains("hello", StringComparison.Ordinal));
        }

        [Test]
        public void Contains_CaseSensitive_MatchFound()
        {
            Assert.IsTrue("Hello World".Contains("Hello", StringComparison.Ordinal));
        }

        [Test]
        public void Contains_Enumerable_IgnoreCase_MatchFound()
        {
            Assert.IsTrue("Hello World".Contains(new[] { "HELLO" }, ignoreCase: true));
        }

        [Test]
        public void Contains_Enumerable_CaseSensitive_NoMatch()
        {
            Assert.IsFalse("Hello World".Contains(new[] { "HELLO" }, ignoreCase: false));
        }

        [Test]
        public void Contains_Enumerable_EmptyKeys_ReturnsFalse()
        {
            Assert.IsFalse("Hello".Contains(new string[0], ignoreCase: true));
        }

        [Test]
        public void Contains_Enumerable_EmptyString_ReturnsFalse()
        {
            Assert.IsFalse("".Contains(new[] { "test" }, ignoreCase: true));
        }

        [Test]
        public void IsContainChinese_WithChinese_ReturnsTrue()
        {
            Assert.IsTrue("Hello你好".IsContainChinese());
        }

        [Test]
        public void IsContainChinese_WithoutChinese_ReturnsFalse()
        {
            Assert.IsFalse("Hello World".IsContainChinese());
        }

        [Test]
        public void IsContainChinese_OnlyChinese_ReturnsTrue()
        {
            Assert.IsTrue("你好世界".IsContainChinese());
        }

        [Test]
        public void IsNullOrEmpty_NullString_ReturnsTrue()
        {
            string s = null;
            Assert.IsTrue(s.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_EmptyString_ReturnsTrue()
        {
            Assert.IsTrue("".IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NonEmptyString_ReturnsFalse()
        {
            Assert.IsFalse("hello".IsNullOrEmpty());
        }

        [Test]
        public void StringToBase64_AndBack()
        {
            string original = "Hello World";
            string encoded = original.StringToBase64();
            string decoded = encoded.Base64ToString();
            Assert.AreEqual(original, decoded);
        }

        [Test]
        public void StringToBase64_EmptyString()
        {
            string encoded = "".StringToBase64();
            Assert.AreEqual("", encoded.Base64ToString());
        }

        [Test]
        public void RemoveFirstChar_NormalString()
        {
            Assert.AreEqual("ello", "Hello".RemoveFirstChar());
        }

        [Test]
        public void RemoveFirstChar_SingleChar()
        {
            Assert.AreEqual("", "H".RemoveFirstChar());
        }

        [Test]
        public void RemoveFirstChar_EmptyString_ReturnsEmpty()
        {
            Assert.AreEqual("", "".RemoveFirstChar());
        }

        [Test]
        public void RemoveFirstChar_NullString_ReturnsNull()
        {
            string s = null;
            Assert.IsNull(s.RemoveFirstChar());
        }

        [Test]
        public void RemoveLastChar_NormalString()
        {
            Assert.AreEqual("Hell", "Hello".RemoveLastChar());
        }

        [Test]
        public void RemoveLastChar_SingleChar()
        {
            Assert.AreEqual("", "H".RemoveLastChar());
        }

        [Test]
        public void RemoveLastChar_EmptyString_ReturnsEmpty()
        {
            Assert.AreEqual("", "".RemoveLastChar());
        }

        [Test]
        public void RemoveLastChar_NullString_ReturnsNull()
        {
            string s = null;
            Assert.IsNull(s.RemoveLastChar());
        }

        [Test]
        public void ToDateTime_ValidDate()
        {
            var result = "2023-06-15".ToDateTime();
            Assert.AreEqual(2023, result.Year);
            Assert.AreEqual(6, result.Month);
            Assert.AreEqual(15, result.Day);
        }

        [Test]
        public void ToDateTime_InvalidDate_ReturnsDefault()
        {
            var result = "not a date".ToDateTime();
            Assert.AreEqual(default(DateTime), result);
        }

        [Test]
        public void ToGuid_ValidGuid()
        {
            string guidStr = "550e8400-e29b-41d4-a716-446655440000";
            var result = guidStr.ToGuid();
            Assert.AreEqual(guidStr, result.ToString());
        }

        [Test]
        public void ToByteArray_BasicString()
        {
            byte[] result = "ABC".ToByteArray();
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(65, result[0]); // 'A'
            Assert.AreEqual(66, result[1]); // 'B'
            Assert.AreEqual(67, result[2]); // 'C'
        }

        [Test]
        public void Replace_WithRegex()
        {
            var regex = new Regex(@"\d+");
            string result = "abc123def456".Replace(regex, "X");
            Assert.AreEqual("abcXdefX", result);
        }

        [Test]
        public void ToDBC_FullWidthToHalfWidth()
        {
            // Full-width 'Ａ' = 65313 (U+FF21), half-width 'A' = 65
            string fullWidth = "\uFF21\uFF22\uFF23";
            string result = fullWidth.ToDBC();
            Assert.AreEqual("ABC", result);
        }

        [Test]
        public void ToDBC_FullWidthSpace_Converted()
        {
            // Full-width space = 12288 (U+3000), half-width space = 32
            string input = "\u3000";
            string result = input.ToDBC();
            Assert.AreEqual(" ", result);
        }

        [Test]
        public void ToDBC_HalfWidthUnchanged()
        {
            string input = "ABC";
            Assert.AreEqual("ABC", input.ToDBC());
        }

        [Test]
        public void Between_ExtractsBetweenDelimiters()
        {
            string input = "start[content]end";
            string result = input.Between("[", "]");
            Assert.AreEqual("content", result);
        }

        [Test]
        public void Between_SameStartAndEnd_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => "test[x[test".Between("[", "["));
        }
    }
}

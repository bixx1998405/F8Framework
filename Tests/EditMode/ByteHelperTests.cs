using System;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class ByteHelperTests
    {
        [Test]
        public void SliceByteArray_ValidSlice_ReturnsCorrectSubarray()
        {
            byte[] input = { 0, 1, 2, 3, 4, 5 };
            byte[] result = ByteHelper.SliceByteArray(input, 2, 3);
            Assert.AreEqual(new byte[] { 2, 3, 4 }, result);
        }

        [Test]
        public void SliceByteArray_FromStart()
        {
            byte[] input = { 10, 20, 30, 40, 50 };
            byte[] result = ByteHelper.SliceByteArray(input, 0, 3);
            Assert.AreEqual(new byte[] { 10, 20, 30 }, result);
        }

        [Test]
        public void SliceByteArray_ToEnd()
        {
            byte[] input = { 10, 20, 30, 40, 50 };
            byte[] result = ByteHelper.SliceByteArray(input, 3, 2);
            Assert.AreEqual(new byte[] { 40, 50 }, result);
        }

        [Test]
        public void SliceByteArray_FullArray()
        {
            byte[] input = { 1, 2, 3 };
            byte[] result = ByteHelper.SliceByteArray(input, 0, 3);
            Assert.AreEqual(new byte[] { 1, 2, 3 }, result);
        }

        [Test]
        public void SliceByteArray_ZeroLength_ReturnsEmpty()
        {
            byte[] input = { 1, 2, 3 };
            byte[] result = ByteHelper.SliceByteArray(input, 1, 0);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void SliceByteArray_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ByteHelper.SliceByteArray(null, 0, 1));
        }

        [Test]
        public void SliceByteArray_InvalidIndex_ThrowsArgumentException()
        {
            byte[] input = { 1, 2, 3 };
            Assert.Throws<ArgumentException>(() => ByteHelper.SliceByteArray(input, -1, 1));
        }

        [Test]
        public void SliceByteArray_IndexPlusLengthExceedsArray_ThrowsArgumentException()
        {
            byte[] input = { 1, 2, 3 };
            Assert.Throws<ArgumentException>(() => ByteHelper.SliceByteArray(input, 2, 3));
        }

        [Test]
        public void ConcatenateByteArrays_TwoArrays()
        {
            byte[] a = { 1, 2 };
            byte[] b = { 3, 4, 5 };
            byte[] result = ByteHelper.ConcatenateByteArrays(a, b);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void ConcatenateByteArrays_ThreeArrays()
        {
            byte[] a = { 1 };
            byte[] b = { 2, 3 };
            byte[] c = { 4, 5, 6 };
            byte[] result = ByteHelper.ConcatenateByteArrays(a, b, c);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Test]
        public void ConcatenateByteArrays_SingleArray()
        {
            byte[] a = { 1, 2, 3 };
            byte[] result = ByteHelper.ConcatenateByteArrays(a);
            Assert.AreEqual(new byte[] { 1, 2, 3 }, result);
        }

        [Test]
        public void ConcatenateByteArrays_EmptyArray_ReturnsEmpty()
        {
            byte[] result = ByteHelper.ConcatenateByteArrays();
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void ConcatenateByteArrays_NullInput_ReturnsEmpty()
        {
            byte[] result = ByteHelper.ConcatenateByteArrays(null);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void ConcatenateByteArrays_WithEmptySubArrays()
        {
            byte[] a = { 1 };
            byte[] b = new byte[0];
            byte[] c = { 2 };
            byte[] result = ByteHelper.ConcatenateByteArrays(a, b, c);
            Assert.AreEqual(new byte[] { 1, 2 }, result);
        }

        [Test]
        public void IntToBytesLittleEndian_Value1_Length4()
        {
            byte[] result = ByteHelper.IntToBytesLittleEndian(1, 4);
            Assert.AreEqual(4, result.Length);
            // Little endian: 1 = 0x01 0x00 0x00 0x00
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(0, result[1]);
        }

        [Test]
        public void IntToBytesLittleEndian_Value256_Length4()
        {
            byte[] result = ByteHelper.IntToBytesLittleEndian(256, 4);
            Assert.AreEqual(4, result.Length);
            // 256 = 0x00 0x01 0x00 0x00 in LE
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
        }

        [Test]
        public void IntToBytesLittleEndian_Length2()
        {
            byte[] result = ByteHelper.IntToBytesLittleEndian(1, 2);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(0, result[1]);
        }

        [Test]
        public void BytesToIntLittleEndian_SimpleValue()
        {
            byte[] bytes = { 1, 0, 0, 0 };
            int result = ByteHelper.BytesToIntLittleEndian(bytes);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void BytesToIntLittleEndian_Value256()
        {
            byte[] bytes = { 0, 1, 0, 0 };
            int result = ByteHelper.BytesToIntLittleEndian(bytes);
            Assert.AreEqual(256, result);
        }

        [Test]
        public void BytesToIntLittleEndian_NullInput_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ByteHelper.BytesToIntLittleEndian(null));
        }

        [Test]
        public void BytesToIntLittleEndian_TooShort_Throws()
        {
            Assert.Throws<ArgumentException>(() => ByteHelper.BytesToIntLittleEndian(new byte[] { 1, 2 }));
        }

        [Test]
        public void IntToBytesAndBack_Roundtrip()
        {
            int original = 12345;
            byte[] bytes = ByteHelper.IntToBytesLittleEndian(original, 4);
            int restored = ByteHelper.BytesToIntLittleEndian(bytes);
            Assert.AreEqual(original, restored);
        }

        [Test]
        public void IntToBytesAndBack_Zero()
        {
            byte[] bytes = ByteHelper.IntToBytesLittleEndian(0, 4);
            int restored = ByteHelper.BytesToIntLittleEndian(bytes);
            Assert.AreEqual(0, restored);
        }

        [Test]
        public void IntToBytesAndBack_NegativeValue()
        {
            int original = -42;
            byte[] bytes = ByteHelper.IntToBytesLittleEndian(original, 4);
            int restored = ByteHelper.BytesToIntLittleEndian(bytes);
            Assert.AreEqual(original, restored);
        }
    }
}

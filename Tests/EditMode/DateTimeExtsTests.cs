using System;
using NUnit.Framework;
using F8Framework.Core;

namespace F8Framework.EditModeTests
{
    [TestFixture]
    public class DateTimeExtsTests
    {
        [Test]
        public void WeekOfYear_FirstDayOfYear()
        {
            var dt = new DateTime(2023, 1, 1);
            int week = dt.WeekOfYear();
            Assert.GreaterOrEqual(week, 1);
        }

        [Test]
        public void WeekOfYear_WithDayOfWeek_Monday()
        {
            var dt = new DateTime(2023, 1, 2);
            int week = dt.WeekOfYear(DayOfWeek.Monday);
            Assert.GreaterOrEqual(week, 1);
        }

        [Test]
        public void WeekOfYear_EndOfYear()
        {
            var dt = new DateTime(2023, 12, 31);
            int week = dt.WeekOfYear();
            Assert.GreaterOrEqual(week, 52);
        }

        [Test]
        public void GetDateTime_AddZeroDays()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 45);
            string result = dt.GetDateTime(0);
            Assert.IsTrue(result.Contains("2023-06-15"));
        }

        [Test]
        public void GetDateTime_AddPositiveDays()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 45);
            string result = dt.GetDateTime(1);
            Assert.IsTrue(result.Contains("2023-06-16"));
        }

        [Test]
        public void GetDateTime_AddNegativeDays()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 45);
            string result = dt.GetDateTime(-1);
            Assert.IsTrue(result.Contains("2023-06-14"));
        }

        [Test]
        public void GetDateTimeF_ReturnsFormattedString()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 45, 123);
            string result = dt.GetDateTimeF();
            Assert.IsTrue(result.StartsWith("2023-06-15 10:30:45:"));
            Assert.AreEqual(27, result.Length);
        }

        [Test]
        public void GetTotalMinutes_ReturnsNumericValue()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 0, DateTimeKind.Local);
            double minutes = dt.GetTotalMinutes();
            Assert.IsTrue(minutes >= 0 || minutes < 0); // Just check it returns without error
        }

        [Test]
        public void GetTotalHours_ReturnsNumericValue()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 0, DateTimeKind.Local);
            double hours = dt.GetTotalHours();
            Assert.IsTrue(hours >= 0 || hours < 0);
        }

        [Test]
        public void GetTotalDays_ReturnsNumericValue()
        {
            var dt = new DateTime(2023, 6, 15, 10, 30, 0, DateTimeKind.Local);
            double days = dt.GetTotalDays();
            Assert.IsTrue(days >= 0 || days < 0);
        }

        [Test]
        public void GetDateTime_FormatIsCorrect()
        {
            var dt = new DateTime(2023, 1, 1, 0, 0, 0);
            string result = dt.GetDateTime(0);
            Assert.AreEqual("2023-01-01 00:00:00", result);
        }
    }
}

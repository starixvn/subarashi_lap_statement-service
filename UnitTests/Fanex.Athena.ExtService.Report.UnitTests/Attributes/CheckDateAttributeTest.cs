using System;
using System.ComponentModel.DataAnnotations;
using Fanex.Athena.ExtService.Report.Attributes;
using Fanex.Athena.ExtService.Report.Constant;
using Fanex.Athena.ExtService.Core.Helper;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Attributes
{
    public class CheckDateAttributeTest
    {
        private readonly CheckDateAttribute _attribute;

        public CheckDateAttributeTest()
        {
            _attribute = new CheckDateAttribute();
        }

        [Fact]
        public void IsValid_WhenValueIsNotDateTime_ReturnsFalse()
        {
            // Arrange
            var value = "not a date";

            // Act
            var result = _attribute.IsValid(value);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenValueIsNull_ReturnsFalse()
        {
            // Arrange
            object value = null;

            // Act
            var result = _attribute.IsValid(value);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenDateIsWithinValidRange_ReturnsTrue()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var validDate = now.AddDays(-3); // Within the 1-8 day range

            // Act
            var result = _attribute.IsValid(validDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenDateIsTooRecent_ReturnsFalse()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var tooRecentDate = now; // Today, which is not allowed (min is 1 day ago)

            // Act
            var result = _attribute.IsValid(tooRecentDate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenDateIsTooOld_ReturnsFalse()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var tooOldDate = now.AddDays(-10); // More than 8 days ago

            // Act
            var result = _attribute.IsValid(tooOldDate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenDateIsExactlyMinimumAllowed_ReturnsTrue()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var minDate = now.AddDays(-ReportOptions.MinSupportedDaysFromToday);
            var exactMinDate = DateTimeHelper.CreateDateTime(minDate.Year, minDate.Month, minDate.Day);

            // Act
            var result = _attribute.IsValid(exactMinDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenDateIsExactlyMaximumAllowed_ReturnsTrue()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var maxDate = now.AddDays(-ReportOptions.MaxSupportedDaysFromToday);
            var exactMaxDate = DateTimeHelper.CreateDateTime(maxDate.Year, maxDate.Month, maxDate.Day);

            // Act
            var result = _attribute.IsValid(exactMaxDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenDateIsOneDayBeforeMinimum_ReturnsFalse()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var beforeMinDate = now.AddDays(-ReportOptions.MinSupportedDaysFromToday + 1);
            var testDate = DateTimeHelper.CreateDateTime(beforeMinDate.Year, beforeMinDate.Month, beforeMinDate.Day);

            // Act
            var result = _attribute.IsValid(testDate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenDateIsOneDayAfterMaximum_ReturnsFalse()
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var afterMaxDate = now.AddDays(-ReportOptions.MaxSupportedDaysFromToday - 1);
            var testDate = DateTimeHelper.CreateDateTime(afterMaxDate.Year, afterMaxDate.Month, afterMaxDate.Day);

            // Act
            var result = _attribute.IsValid(testDate);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(-2)]  // 2 days ago - valid
        [InlineData(-3)]  // 3 days ago - valid
        [InlineData(-4)]  // 4 days ago - valid
        [InlineData(-5)]  // 5 days ago - valid
        [InlineData(-6)]  // 6 days ago - valid
        [InlineData(-7)]  // 7 days ago - valid
        public void IsValid_WithValidDayOffsets_ReturnsTrue(int dayOffset)
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var testDate = now.AddDays(dayOffset);
            var dateToTest = DateTimeHelper.CreateDateTime(testDate.Year, testDate.Month, testDate.Day);

            // Act
            var result = _attribute.IsValid(dateToTest);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]   // Today - invalid
        [InlineData(-9)]  // 9 days ago - invalid
        [InlineData(-10)] // 10 days ago - invalid
        public void IsValid_WithInvalidDayOffsets_ReturnsFalse(int dayOffset)
        {
            // Arrange
            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var testDate = now.AddDays(dayOffset);
            var dateToTest = DateTimeHelper.CreateDateTime(testDate.Year, testDate.Month, testDate.Day);

            // Act
            var result = _attribute.IsValid(dateToTest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FormatErrorMessage_ReturnsFormattedMessage()
        {
            // Arrange
            var propertyName = "TestDate";
            _attribute.ErrorMessage = "The field {0} must be between {1} and {2}.";

            // Act
            var result = _attribute.FormatErrorMessage(propertyName);

            // Assert
            Assert.Contains(propertyName, result);
            Assert.Contains("must be between", result);
        }

        [Fact]
        public void FormatErrorMessage_IncludesCorrectDateRange()
        {
            // Arrange
            var propertyName = "TestDate";
            _attribute.ErrorMessage = "The field {0} must be between {1} and {2}.";

            var now = DateTimeHelper.UtcNow.AddHours(-4);
            var expectedMinDate = now.AddDays(-ReportOptions.MaxSupportedDaysFromToday);
            var expectedMaxDate = now.AddDays(-ReportOptions.MinSupportedDaysFromToday);
            var minDateFormatted = DateTimeHelper.CreateDateTime(expectedMinDate.Year, expectedMinDate.Month, expectedMinDate.Day);
            var maxDateFormatted = DateTimeHelper.CreateDateTime(expectedMaxDate.Year, expectedMaxDate.Month, expectedMaxDate.Day);

            // Act
            var result = _attribute.FormatErrorMessage(propertyName);

            // Assert
            Assert.Contains(minDateFormatted.ToString(), result);
            Assert.Contains(maxDateFormatted.ToString(), result);
        }
    }
}
using System;
using System.IO;
using Fanex.Athena.ExtService.Report.Constant;
using Fanex.Athena.ExtService.Report.Converters;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Converters
{
    public class DecimalJsonConverterTest
    {
        private readonly DecimalJsonConverter _converter;

        public DecimalJsonConverterTest()
        {
            _converter = new DecimalJsonConverter();
        }

        [Theory]
        [InlineData("123.45", 123.45)]
        [InlineData("0", 0)]
        [InlineData("999999.99", 999999.99)]
        [InlineData("-123.45", -123.45)]
        public void ReadJson_WithValidDecimalValues_ReturnsCorrectDecimal(string jsonValue, decimal expected)
        {
            // Arrange
            using var stringReader = new StringReader($"\"{jsonValue}\"");
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read(); // Read the value

            // Act
            var result = _converter.ReadJson(jsonReader, typeof(decimal), 0m, false, null);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(0)]
        [InlineData(-456)]
        public void ReadJson_WithIntegerValues_ReturnsCorrectDecimal(int jsonValue, decimal expected = default)
        {
            // Arrange
            if (expected == default) expected = jsonValue;
            
            using var stringReader = new StringReader(jsonValue.ToString());
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read(); // Read the value

            // Act
            var result = _converter.ReadJson(jsonReader, typeof(decimal), 0m, false, null);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(123.45, "123.45")]
        [InlineData(0, "0.00")]
        [InlineData(999999.99, "999999.99")]
        [InlineData(-123.45, "-123.45")]
        [InlineData(100, "100.00")]
        public void WriteJson_WithDecimalValues_WritesFormattedValue(decimal value, string expectedOutput)
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void WriteJson_WithZeroValue_WritesCorrectFormat()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            var value = 0m;

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal("0.00", result);
        }

        [Fact]
        public void WriteJson_WithSmallDecimalValue_WritesCorrectFormat()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            var value = 0.01m;

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal("0.01", result);
        }

        [Theory]
        [InlineData(1.234, "1.23")]  // Rounds down
        [InlineData(1.235, "1.24")]  // Rounds up (banker's rounding)
        [InlineData(1.999, "2.00")]  // Rounds up
        public void WriteJson_WithMoreThanTwoDecimalPlaces_RoundsToTwoDecimalPlaces(decimal value, string expected)
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteJson_UsesReportOptionsNumberFormatPattern()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            var value = 123.4m;

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            var expectedFormat = value.ToString(ReportOptions.NumberFormatPattern);
            Assert.Equal(expectedFormat, result);
        }

        [Fact]
        public void ReadJson_WithNullValue_ReturnsZero()
        {
            // Arrange
            using var stringReader = new StringReader("null");
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read(); // Read the null value

            // Act
            var result = _converter.ReadJson(jsonReader, typeof(decimal), 0m, false, null);

            // Assert
            Assert.Equal(0m, result);
        }

        [Theory]
        [InlineData("123.45")]
        [InlineData("0.00")]
        [InlineData("-999.99")]
        public void ReadJson_WithStringValues_ConvertsToDecimal(string stringValue)
        {
            // Arrange
            var expectedDecimal = Convert.ToDecimal(stringValue);
            using var stringReader = new StringReader($"\"{stringValue}\"");
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read(); // Read the value

            // Act
            var result = _converter.ReadJson(jsonReader, typeof(decimal), 0m, false, null);

            // Assert
            Assert.Equal(expectedDecimal, result);
        }

        [Fact]
        public void ReadJson_WithExistingValue_IgnoresExistingValue()
        {
            // Arrange
            var existingValue = 999.99m;
            var newValue = 123.45m;
            using var stringReader = new StringReader(newValue.ToString());
            using var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read(); // Read the value

            // Act
            var result = _converter.ReadJson(jsonReader, typeof(decimal), existingValue, true, null);

            // Assert
            Assert.Equal(newValue, result);
            Assert.NotEqual(existingValue, result);
        }

        [Theory]
        [InlineData(12345.67, "12345.67")]
        [InlineData(0.1, "0.10")]
        [InlineData(-0.01, "-0.01")]
        public void WriteJson_WithVariousDecimalValues_MaintainsCorrectPrecision(decimal value, string expected)
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            var result = stringWriter.ToString();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteJson_WithLargeDecimalValue_HandlesCorrectly()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            var value = decimal.MaxValue;

            // Act & Assert - Should not throw
            _converter.WriteJson(jsonWriter, value, null);
            var result = stringWriter.ToString();
            Assert.NotEmpty(result);
        }

        [Fact]
        public void WriteJson_WithMinDecimalValue_HandlesCorrectly()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            var value = decimal.MinValue;

            // Act & Assert - Should not throw
            _converter.WriteJson(jsonWriter, value, null);
            var result = stringWriter.ToString();
            Assert.NotEmpty(result);
        }
    }
}
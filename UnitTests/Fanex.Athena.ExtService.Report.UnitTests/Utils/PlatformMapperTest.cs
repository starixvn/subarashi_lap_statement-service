using Fanex.Athena.ExtService.Report.Utils;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Utils
{
    public class PlatformMapperTest
    {
        [Theory]
        [InlineData("Mobile", "Mobile")]
        [InlineData("Wap", "Wap")]
        [InlineData("Web", "Web")]
        [InlineData("Unknown", "Unknown")]
        [InlineData("", "Unknown")]
        [InlineData(null, "Unknown")]
        public void GetPlatform_ShouldReturnCorrectPlatform(string input, string expected)
        {
            // Act
            var result = PlatformMapper.GetPlatform(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
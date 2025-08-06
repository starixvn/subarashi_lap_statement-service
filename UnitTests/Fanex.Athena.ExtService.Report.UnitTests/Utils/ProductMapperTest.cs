using Fanex.Athena.ExtService.Report.Utils;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Utils
{
    public class ProductMapperTest
    {
        [Theory]
        [InlineData(1, "Sports")]
        [InlineData(2, "Casino")]
        [InlineData(3, "Games")]
        [InlineData(4, "Keno")]
        [InlineData(5, "Lottery")]
        [InlineData(6, "Vsports")]
        [InlineData(0, "Unknown")]
        [InlineData(100, "Unknown")]
        public void GetProductName_ShouldReturnCorrectProductName(byte productId, string expected)
        {
            // Act
            var result = ProductMapper.GetProductName(productId);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
namespace Fanex.Athena.ExtService.Core.UnitTests.Attributes
{
    using Core.Attributes;
    using Xunit;

    public class RateLimitAttributeTest
    {
        [Fact]
        public void Constructor_DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var attribute = new RateLimitAttribute();

            // Assert
            Assert.Equal(0, attribute.MaxRequests);
            Assert.Equal(0, attribute.TimeWindowInSeconds);
        }

        [Theory]
        [InlineData(10, 20)]
        [InlineData(100, 120)]
        public void Constructor_WithParameters_ShouldSetProperties(int maxRequests, int timeWindowInSeconds)
        {
            // Arrange & Act
            var attribute = new RateLimitAttribute
            {
                MaxRequests = maxRequests,
                TimeWindowInSeconds = timeWindowInSeconds
            };

            // Assert
            Assert.Equal(maxRequests, attribute.MaxRequests);
            Assert.Equal(timeWindowInSeconds, attribute.TimeWindowInSeconds);
        }
    }
}
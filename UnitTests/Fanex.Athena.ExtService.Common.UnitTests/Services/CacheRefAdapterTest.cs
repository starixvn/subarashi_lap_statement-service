using static System.String;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    using Common.Services;
    using Xunit;

    public class CacheRefAdapterTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData("")]
        public void GetBetTypeName_WithInvalidBetTypeId_ReturnEmptyString(object betTypeId)
        {
            //Arrange
            var cacheRefAdapter = new CacheRefAdapter();

            //Action
            var result = cacheRefAdapter.GetBetTypeName(betTypeId);

            //Assert
            Assert.Equal(Empty, result);
        }
    }
}
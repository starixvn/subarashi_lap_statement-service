namespace Fanex.Athena.ExtService.Common.UnitTests
{
    using Models.DataDomain;
    using Models.DTO;
    using Xunit;

    public class SystemHelperTest
    {
        [Fact]
        public void GenerateAgentListXml_WithMultipleCurrencies_ShouldGenerateCorrectXml()
        {
            // Arrange
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[]
                {
                    new CurrencyInfo { AgentId = 123, CurrencyId = 1 },
                    new CurrencyInfo { AgentId = 456, CurrencyId = 2 }
                }
            };

            // Act
            var result = SystemHelper.GenerateAgentListXml(licenseeInfo);

            // Assert
            var expected = "<ROOT><Agent AgentId=\"123\" Currency=\"1\"/><Agent AgentId=\"456\" Currency=\"2\"/></ROOT>";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateAgentListXml_WithTestData_ShouldIncludeUusCurrency()
        {
            // Arrange
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[]
                {
                    new CurrencyInfo { AgentId = 123, CurrencyId = 1 },
                    new CurrencyInfo { AgentId = 999, CurrencyId = CurrencyType.Uus }
                }
            };

            // Act
            var result = SystemHelper.GenerateAgentListXml(licenseeInfo, isExcludeTestData: false);

            // Assert
            var expected = "<ROOT><Agent AgentId=\"123\" Currency=\"1\"/><Agent AgentId=\"999\" Currency=\"20\"/></ROOT>";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateAgentListXml_ExcludeTestData_ShouldNotIncludeUusCurrency()
        {
            // Arrange
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[]
                {
                    new CurrencyInfo { AgentId = 123, CurrencyId = 1 },
                    new CurrencyInfo { AgentId = 999, CurrencyId = CurrencyType.Uus }
                }
            };

            // Act
            var result = SystemHelper.GenerateAgentListXml(licenseeInfo, isExcludeTestData: true);

            // Assert
            var expected = "<ROOT><Agent AgentId=\"123\" Currency=\"1\"/></ROOT>";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateAgentListXml_WithEmptyCurrencies_ShouldReturnEmptyRootXml()
        {
            // Arrange
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new CurrencyInfo[0]
            };

            // Act
            var result = SystemHelper.GenerateAgentListXml(licenseeInfo);

            // Assert
            var expected = "<ROOT></ROOT>";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateAgentListXml_WithSingleCurrency_ShouldGenerateCorrectXml()
        {
            // Arrange
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[]
                {
                    new CurrencyInfo { AgentId = 123, CurrencyId = 1 }
                }
            };

            // Act
            var result = SystemHelper.GenerateAgentListXml(licenseeInfo);

            // Assert
            var expected = "<ROOT><Agent AgentId=\"123\" Currency=\"1\"/></ROOT>";
            Assert.Equal(expected, result);
        }
    }
}
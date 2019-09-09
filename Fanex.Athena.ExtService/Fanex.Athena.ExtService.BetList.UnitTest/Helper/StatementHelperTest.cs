using Fanex.Athena.ExtService.BetList.Helper;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.DTO;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTest.Helper
{
    public class StatementHelperTest
    {
        private readonly Mock<IStatementValue> _statementValue;
        const decimal PreviousBalance = 10;
        const decimal TotalAmt = 100;
        const decimal TurnOvert = 200;
        const decimal Comm = 0.5m;
        public StatementHelperTest()
        {
            _statementValue = new Mock<IStatementValue>();
            _statementValue.Setup(x => x.Commission).Returns(Comm);
            _statementValue.Setup(x => x.TotalAmt).Returns(TotalAmt);
            _statementValue.Setup(x => x.TurnOver).Returns(TurnOvert);

        }
        [Theory]
        [InlineData(TransDescValue.BetStatement)]
        [InlineData(TransDescValue.VoidCancell)]
        [InlineData(TransDescValue.Outstanding)]
        [InlineData("Any")]
        public void GetBalance_WhenCallWithBETTransType_ReturnCorrectValue(
            string transDesc)
        {
            //Arrange
            _statementValue.Setup(x => x.TransType).Returns(TransTypeValue.BET);
            _statementValue.Setup(x => x.TransDesc).Returns(transDesc);
            var expectedValue = GetExpectedValueWithBetTransType();
            //Action
            var result = StatementHelper.GetBalance(PreviousBalance, _statementValue.Object);

            //Assert

            Assert.Equal(expectedValue, result);
        }

        private object GetExpectedValueWithBetTransType()
        {
            if (_statementValue.Object.TransDesc == TransDescValue.BetStatement)
            {
                return PreviousBalance + _statementValue.Object.TotalAmt + _statementValue.Object.Commission;
            }

            if (_statementValue.Object.TransDesc == TransDescValue.VoidCancell)
            {
                return PreviousBalance;
            }

            if (_statementValue.Object.TransDesc == TransDescValue.Outstanding)
            {
                return PreviousBalance - _statementValue.Object.TurnOver;
            }
            return 0m;
        }

        [Theory]
        [InlineData(TransTypeValue.BAL)]
        [InlineData(TransTypeValue.GP)]
        [InlineData("ANY")]
        public void GetBalance_WhenCallWithTransType_ReturnCorrectValue(string transtype)
        {
            //Arrange
            var expectedValue = transtype == TransTypeValue.BAL ? TotalAmt : PreviousBalance + TotalAmt;
            _statementValue.Setup(x => x.TransType).Returns(transtype);

            //Action
            var result = StatementHelper.GetBalance(PreviousBalance, _statementValue.Object);

            //Assert
            Assert.Equal(expectedValue, result);
        }
    }
}

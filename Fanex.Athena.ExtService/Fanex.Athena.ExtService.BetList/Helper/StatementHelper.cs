using System;

using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.DTO;

namespace Fanex.Athena.ExtService.BetList.Helper
{
    public static class StatementHelper
    {
        public static decimal GetBalance(Decimal previousBalance, IStatementValue item)
        {
            switch (item.TransType?.ToUpperInvariant())
            {
                case TransTypeValue.BAL:
                    return item.TotalAmt;
                case TransTypeValue.GP:
                    return previousBalance + item.TotalAmt;
                case TransTypeValue.BET:
                    switch (item.TransDesc)
                    {
                        case TransDescValue.BetStatement:
                            return previousBalance + item.TotalAmt + item.Commission;
                        case TransDescValue.Outstanding:
                            return previousBalance - item.TurnOver;
                        case TransDescValue.VoidCancell:
                            return previousBalance;
                        default:
                            return 0;
                    }
                default: return previousBalance + item.TotalAmt;
            }

        }
        public static decimal GetCreditDebit(IStatementValue item)
        {
            return item.TransDesc.Equals(TransDescValue.Outstanding, StringComparison.OrdinalIgnoreCase)
                || item.TransType.Equals(TransTypeValue.BAL) ? 0 : item.TotalAmt;
        }

        public static bool HideBetListLink(string productCode)
        {
            const string AGCasino = "AGC";
            const string GoldDeluxe = "GD";
            const string Exchange = "BX";

            return productCode == AGCasino || productCode == GoldDeluxe || productCode == Exchange;
        }
    }
}

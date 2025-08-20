namespace Fanex.Athena.ExtService.BetList.Helper
{
    using System;
    using System.Collections.Generic;
    using Common.Services;
    using Models.DataDomain;
    using Models.DTO;

    public static class StatementHelper
    {
        private const int DDEPBettypeId = 22001;
        private const int DWITBettypeId = 22002;

        public static decimal GetStatementBalance(Decimal previousBalance, IStatementValue item)
        {
            switch (item.TransType?.ToUpperInvariant())
            {
                case TransTypeValue.BAL:
                    return item.TotalAmt;

                case TransTypeValue.GP:
                    return previousBalance + item.TotalAmt;

                case TransTypeValue.BET:
                    return GetBalanceForBETTranstype(previousBalance, item);

                default: return previousBalance + item.TotalAmt;
            }
        }

        public static decimal GetBettingBalance(Decimal previousBalance, IStatementValue item)
        {
            return item.TransDesc == TransDescValue.PromotionPrize
                ? previousBalance + item.TotalAmt
                : GetBalanceForBETTranstype(previousBalance, item);
        }

        private static decimal GetBalanceForBETTranstype(decimal previousBalance, IStatementValue item)
        {
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
        }

        public static decimal GetCreditDebit(IStatementValue item)
        {
            return item.TransDesc.Equals(TransDescValue.Outstanding, StringComparison.OrdinalIgnoreCase)
                || item.TransType.Equals(TransTypeValue.BAL) ? 0 : item.TotalAmt;
        }

        public static bool HideBetListLink(string productCode)
        {
            const string AGCasino = "AGC";
            const string Exchange = "BX";

            return productCode == AGCasino || productCode == Exchange;
        }

        public static string GetStatementRemark(string transDesc, string transType, int betTypeId = 0)
        {
            if (TransDescValue.RemarkValues.ContainsKey(transDesc))
            {
                return $"- {TransDescValue.RemarkValues[transDesc].Invoke()}";
            }

            if (betTypeId != 0)
            {
                return new CacheRefAdapter().GetBetTypeName(betTypeId);
            }

            return transType.ToUpperInvariant();
        }

        public static bool ShouldHideTransferTrans(bool isEnableSingleWallet, int bettypeId)
            => isEnableSingleWallet && (bettypeId == DDEPBettypeId || bettypeId == DWITBettypeId);

        public static bool ShouldHideWinLossDate(string transDesc)
          => string.Equals(TransDescValue.AccumulatedBalance, transDesc, StringComparison.InvariantCultureIgnoreCase);

        public static bool ShouldHighlight(string transDesc)
         => string.Equals(TransDescValue.AccumulatedBalance, transDesc, StringComparison.InvariantCultureIgnoreCase);

        public static string GetTransDescText(string transDesc)
        => TransDescValue.RemarkValues.ContainsKey(transDesc)
                ? TransDescValue.RemarkValues[transDesc].Invoke()
                : string.Empty;

        public static string GetBetingStatementRemark(string transDescText, string productCode, Dictionary<string, string> productResources)
        {
            const string SabaPromotion = "SPR";
            var productName = productResources.GetValueOrDefault(productCode, string.Empty);

            return productCode == SabaPromotion
                ? productName
                : $"{productName} {transDescText}";
        }

        public static bool ShowBetingStatementLink(string transDesc, string transType)
        => (transType == TransTypeValue.BET || transType == TransTypeValue.PROMO) && TransDescValue.RemarkValues.ContainsKey(transDesc);
    }
}
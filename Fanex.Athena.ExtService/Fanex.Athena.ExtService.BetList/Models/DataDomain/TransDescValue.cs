using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.Resources;

namespace Fanex.Athena.ExtService.BetList.Models.DataDomain
{
    public class TransDescValue
    {
        public const string Outstanding = "Outstanding_Bet";

        public const string GrandPrize = "Grand_Prize";

        public const string VoidCancell = "Void_Cancelled_Bet";
        public const string BetStatement = "Bet_Statement";
        public const string OpeningBalance = "Opening_Balance";

        public static readonly IDictionary<string, Func<string>> RemarkValues = new Dictionary<string, Func<string>> {

            { BetStatement, () => ViewResources.BettingStatement },
            { VoidCancell,() => ViewResources.Void_CancelledBet },
            { Outstanding,() => ViewResources.OutstandingBet },
            { GrandPrize,() => ViewResources.GrandPrize },
            { OpeningBalance,() => ViewResources.OPENINGBALANCE }
        };

        public static readonly IDictionary<string, int> MaptoModeView = new Dictionary<string, int> {

            { BetStatement,1 },
            { VoidCancell,2 },
            { Outstanding,3 },

        };
    }
}

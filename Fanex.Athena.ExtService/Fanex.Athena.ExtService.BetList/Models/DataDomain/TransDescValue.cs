using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.Resources;

namespace Fanex.Athena.ExtService.BetList.Models.DataDomain
{
    public static class TransDescValue
    {
        public const string BetStatement = "Bet_Statement";

        public const string GrandPrize = "Grand_Prize";

        public const int ModeViewBetStatement = 1;
        public const int ModeViewCancell = 2;
        public const int ModeViewOutstanding = 3;
        public const string OpeningBalance = "Opening_Balance";
        public const string Outstanding = "Outstanding_Bet";

        public const string VoidCancell = "Void_Cancelled_Bet";

        public static readonly IDictionary<string, int> MaptoModeView = new Dictionary<string, int> {

            { BetStatement, ModeViewBetStatement },
            { VoidCancell, ModeViewCancell },
            { Outstanding, ModeViewOutstanding },

        };

        public static readonly IDictionary<string, Func<string>> RemarkValues = new Dictionary<string, Func<string>> {

            { BetStatement, () => ViewResources.BettingStatement },
            { VoidCancell,() => ViewResources.Void_CancelledBet },
            { Outstanding,() => ViewResources.OutstandingBet },
            { GrandPrize,() => ViewResources.GrandPrize },
            { OpeningBalance,() => ViewResources.OPENINGBALANCE }
        };
    }
}

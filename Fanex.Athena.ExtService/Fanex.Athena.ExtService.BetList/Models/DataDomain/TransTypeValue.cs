using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.DataDomain
{
    public class TransTypeValue
    {
        public const string BAL = "BAL";
        public const string BET = "BET";
        public const string GP = "GP";

        public static readonly IDictionary<string, int> BaseCurrencyList = new Dictionary<string, int> {

            { TransDescValue.BetStatement,1 },
            { TransDescValue.VoidCancell,2 },
            { TransDescValue.Outstanding,3 },

        };
    }
}

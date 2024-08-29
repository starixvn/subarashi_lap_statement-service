namespace Fanex.Athena.ExtService.Report.Utils
{
    using System;
    using System.Collections.Generic;

    public static class ProductMapper
    {
        private const string DefaultPlatformName = "";

        private static readonly Dictionary<string, string> ProductList = new(StringComparer.OrdinalIgnoreCase)
        {
            { "0", "All Product" },
            { "LT", "Lottery" },
            { "PP", "PP" },
            { "VRG", "Virtual Games" },
            { "NG", "Number Game" },
            { "KN", "RNG Keno" },
            { "TG", "Table Game" },
            { "AP", "AdvantPlay" },
            { "APM", "AdvantPlay Mini" },
            { "MXG", "MaxGame" },
            { "VS", "Virtual Sports" },
            { "SB", "Sportsbook" }
        };

        public static string GetProductName(string productCode)
            => ProductList.GetValueOrDefault(productCode, DefaultPlatformName);
    }
}
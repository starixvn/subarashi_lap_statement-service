namespace Fanex.Athena.ExtService.Report.Utils
{
    using System.Collections.Generic;

    public static class PlatformMapper
    {
        private const string DefaultPlatformName = "Others";

        private static readonly Dictionary<int, string> PlatformList = new()
        {
            { 0, "All Platform" },
            { 1, "Desktop New Asia" },
            { 2, "Desktop China" },
            { 3, "Desktop Others" },
            { 4, "Mobile Standard" },
            { 5, "Mobile Euro" },
            { 6, "Mobile Compact" },
            { 7, "Mobile Others" },
            { 8, "E-Sports Desktop" },
            { 9, "E-Sports Mobile" },
            { 10, "E-Sports APP" },
            { 11, "Mobile APP" },
            { 12, "Native APP" },
            { 13, "Tablet" },
            { 14, "Deep Link" },
            { 15, "WAP" },
            { 88, "Others" }
        };

        public static string GetPlatformName(int platformType)
            => PlatformList.GetValueOrDefault(platformType, DefaultPlatformName);
    }
}
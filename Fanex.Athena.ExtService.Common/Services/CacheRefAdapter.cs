namespace Fanex.Athena.ExtService.Common.Services
{
    using System;
    using System.Globalization;
    using RemotingRef;

    public class CacheRefAdapter : ICacheRefAdapter
    {
        private readonly Lazy<IRemotingRefService> remotingRefService = new Lazy<IRemotingRefService>(() => new RemotingRefService());

        public static CacheRefAdapter Instance { get; } = new CacheRefAdapter();

        public IRemotingRefService RemotingRefService => remotingRefService.Value;

        public string GetBetTypeName(object betTypeId)
        {
            if (!IsValidId(betTypeId))
            {
                return string.Empty;
            }

            return RemotingRefService.GetBetTypeName(betTypeId.ToString(), GetCurrentLanguageCode());
        }

        private static bool IsValidId(object id)
            => id != null && id.ToString() != string.Empty && id.ToString() != "0";

        private static string GetCurrentLanguageCode(string languageCode = null)
            => string.IsNullOrEmpty(languageCode)
            ? CultureInfo.CurrentUICulture.IetfLanguageTag
            : languageCode;
    }
}
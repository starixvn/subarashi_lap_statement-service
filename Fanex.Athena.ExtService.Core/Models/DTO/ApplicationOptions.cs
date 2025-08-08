namespace Fanex.Athena.ExtService.Core.Models.DTO
{
    using System.Collections.Generic;

    public class ApplicationOptions
    {
        public IList<string> WhiteList { get; set; }

        public RedisConfiguration Redis { get; set; }

        public string LogCategory { get; set; }

        public string SentryDsn { get; set; }
    }
}
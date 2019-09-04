using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Model.DTO;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Common.DTO
{
    public class LicenseeInfo
    {
        [JsonProperty("comm")]
        public bool IsAllowedCustomerComm { get; set; }
        [JsonProperty("name")]
        public string SiteName { get; set; }

        [JsonProperty("cur")]
        public CurrencyInfo[] Currencies { get; set; }
    }
}

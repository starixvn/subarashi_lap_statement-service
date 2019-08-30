using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Shared.Model.DTO;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Shared.DTO
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

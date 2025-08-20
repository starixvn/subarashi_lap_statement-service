namespace Fanex.Athena.ExtService.Common.Models.DTO
{
    using Newtonsoft.Json;

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
namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Newtonsoft.Json;

    public class PlayerClosingBalance
    {
        [JsonProperty(PropertyName = "Username")]
        public string Nickname { get; set; }

        [JsonIgnore]
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}
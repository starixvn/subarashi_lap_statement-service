namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Converters;
    using Newtonsoft.Json;

    public class PlayerReportBase
    {
        [JsonProperty(PropertyName = "Username")]
        public string Nickname { get; set; }

        public int BetCount { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Turnover { get; set; }

        [JsonProperty(PropertyName = "PlayerWL")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Winloss { get; set; }
    }
}
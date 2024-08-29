namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Newtonsoft.Json;

    public class PlayerReportBase
    {
        [JsonProperty(PropertyName = "Username")]
        public string Nickname { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal PlayerWL { get; set; }
    }
}
namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Newtonsoft.Json;

    public class PlayerReportByProduct : PlayerReportBase
    {
        [JsonIgnore]
        public string ProductCode { get; set; }

        public string Product { get; set; }
    }
}
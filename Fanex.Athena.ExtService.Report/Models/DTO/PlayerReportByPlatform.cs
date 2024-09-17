namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Newtonsoft.Json;

    public class PlayerReportByPlatform : PlayerReportBase
    {
        [JsonIgnore]
        public int PlatformType { get; set; }

        public string Platform { get; set; }
    }
}
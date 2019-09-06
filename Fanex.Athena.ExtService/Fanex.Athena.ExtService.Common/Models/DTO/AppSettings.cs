namespace Fanex.Athena.ExtService.Common.Models.DTO
{
    public class AppSettings
    {
        public string LogCategory { get; set; }
        public string RabbitMQ { get; set; }
        public string Environment { get; set; }
        public string BetListApiHostUrl { get; set; }
        public string BetListApiTokenValue { get; set; }
        public string BetListApiClientTimeOutMinutes { get; set; }
    }
}


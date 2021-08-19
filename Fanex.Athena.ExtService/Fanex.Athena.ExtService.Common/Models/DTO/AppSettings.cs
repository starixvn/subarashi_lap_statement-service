using System.Collections.Generic;

namespace Fanex.Athena.ExtService.Common.Models.DTO
{
    public class AppSettings
    {
        public string BetListApiHostUrl { get; set; }
        public string BetListApiTokenValue { get; set; }
        public string Environment { get; set; }
        public string LicenseeOfferTxIDColumn { get; set; }
        public string LicenseeOfferShowIp { get; set; }
        public bool IsShowIpToAllLicensee { get; set; }
    }
}
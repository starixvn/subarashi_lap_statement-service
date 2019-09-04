using Fanex.Athena.ExtService.Common.DTO;

namespace Fanex.Athena.ExtService.Common.Model.ViewModels
{
    public class BaseViewModel
    {
        public LicenseeInfo Info { get; set; }
        public string AccessToken { get; set; }
        public string Lang { get; set; }
        public string BaseCurrency { get; set; }
    }
}

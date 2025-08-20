namespace Fanex.Athena.ExtService.Common.Models.ViewModels
{
    using DTO;

    public class BaseViewModel
    {
        public LicenseeInfo Info { get; set; }

        public string AccessToken { get; set; }

        public string Lang { get; set; }

        public string BaseCurrency { get; set; }

        public byte ViewType { get; set; }
    }
}
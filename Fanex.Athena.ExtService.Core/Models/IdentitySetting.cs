namespace Fanex.Athena.ExtService.Core.Models
{
    public class IdentitySetting
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TokenPath { get; set; }

        public string UserInfoPath { get; set; }

        public string Scope { get; set; }
    }
}

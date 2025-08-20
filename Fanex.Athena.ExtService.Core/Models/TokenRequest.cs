namespace Fanex.Athena.ExtService.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class TokenRequest
    {
        private const int MaxLengthOfUserName = 50;
        private const int MaxLengthOfPassword = 16;

        [MaxLength(MaxLengthOfUserName)]
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [MaxLength(MaxLengthOfPassword)]
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        public bool IsShowIpvn { get; set; } = true;
    }
}
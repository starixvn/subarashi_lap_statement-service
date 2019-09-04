using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Common.Model.DTO
{
    public class CurrencyInfo
    {
        [JsonProperty("Id")]
        public int CurrencyId { get; set; }
        [JsonProperty("Name")]
        public string CurrencyName { get; set; }
        [JsonProperty("aId")]
        public int AgentId { get; set; }
    }
}

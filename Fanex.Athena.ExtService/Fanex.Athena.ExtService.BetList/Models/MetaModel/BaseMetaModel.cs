using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class BaseMetaModel : IBaseMetaModel
    {
        public string AgentListXml { get; set; }
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public interface IBaseBetListMetaModel : IBaseMetaModel
    {
        IDictionary<int, string> UserCurrencyList { get; set; }
        bool HasPlayerCom { get; set; }
        string AgentList { get; set; }
    }
}

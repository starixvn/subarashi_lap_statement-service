using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class ColossusBetDetailMetaModel : IBaseMetaModel, ICustomerMetaModel
    {
        public int UserId { get; set; } = 1;

        public int CustId { get; set; }

        public long RefNo { get; set; }

        public long BetCheck { get; set; }
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
        public string AgentListXml { get; set; }

    }
}

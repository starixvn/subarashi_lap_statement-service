using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class ColossusBetDetailMetaModel : BaseMetaModel
    {
        public int UserId = 1;

        public int CustId { get; set; }

        public long RefNo { get; set; }

        public long BetCheck { get; set; }

    }
}

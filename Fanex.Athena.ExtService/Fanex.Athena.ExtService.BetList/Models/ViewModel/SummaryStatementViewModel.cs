using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class SummaryStatementViewModel : BaseViewModel
    {
        public IEnumerable<SummaryStatement> DataList { get; set; }
        public int TotalRows { get; set; }
        public int CustId { get; set; }

    }
}

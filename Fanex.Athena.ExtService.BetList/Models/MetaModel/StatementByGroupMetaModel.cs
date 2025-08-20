namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    using System;
    using System.Linq;
    using Core.Helper;
    using DataDomain;
    using Microsoft.AspNetCore.Mvc;

    public class StatementByGroupMetaModel : SummaryStatementMetaModel
    {
        [FromQuery(Name = "wd")]
        public DateTime WinlostDate { get; set; }

        [FromQuery(Name = "mv")]
        public int ModeView { get; set; }

        public new bool IsValid => CustId > 0 && TransDescValue.MaptoModeView.Values.Contains(ModeView)
                                              && WinlostDate > DateTimeHelper.MinValue;
    }
}
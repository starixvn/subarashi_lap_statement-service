using System;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class StatementByGroupMetaModel : IBaseMetaModel
    {
        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "wd")]
        public DateTime WinlostDate { get; set; }

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; }

        [FromQuery(Name = "mv")]
        public int ModeView { get; set; }
        public string Lang { get; set; }

        public bool IsValid => CustId > 0 && TransDescValue.MaptoModeView.Values.Contains(ModeView)
            && WinlostDate > DateTime.MinValue;
    }
}
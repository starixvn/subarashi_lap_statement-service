using System;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class StatementByGroupMetaModel : IBaseMetaModel, ICustomerMetaModel
    {
        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "wd")]
        public DateTime WinlostDate { get; set; }

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;

        [FromQuery(Name = "mv")]
        public int ModeView { get; set; }
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsValid => CustId > 0 && TransDescValue.MaptoModeView.Values.Contains(ModeView)
            && WinlostDate > DateTime.MinValue;
    }
}
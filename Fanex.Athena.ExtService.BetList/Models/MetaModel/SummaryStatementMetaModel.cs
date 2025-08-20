namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    using System;
    using Common.Models.DataDomain;
    using Common.Models.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class SummaryStatementMetaModel : IBaseMetaModel, ICustomerMetaModel
    {
        private const int MonthsLimit = 2;

        [FromQuery(Name = "vt")]
        public byte ViewType { get; set; } = SystemConstants.DefaultViewType;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "fd")]
        public DateTime FromDate { get; set; }

        [FromQuery(Name = "td")]
        public DateTime ToDate { get; set; }

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = SystemConstants.DefaultPageSize;

        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;

        [FromQuery(Name = "sb")]
        public bool ShowBreadCrumb { get; set; }

        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsDesktopViewOnly { get; set; }

        public bool IsValid
        {
            get
            {
                var pagingValidation = PageIndex > 0 && PageSize > 0;
                var mainValidation = CustId > 0 && FromDate <= ToDate && ((ToDate.Month - FromDate.Month) + 12 * (ToDate.Year - FromDate.Year) + 1) <= MonthsLimit;

                return pagingValidation && mainValidation;
            }
        }
    }
}
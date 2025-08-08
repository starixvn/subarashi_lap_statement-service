namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class SummaryStatementViewModel : BaseViewModel
    {
        public IEnumerable<SummaryStatement> DataList { get; set; }

        public int TotalRows { get; set; }

        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsDesktopViewOnly { get; set; }

        public bool IsShowIpvn { get; internal set; }

        public bool IsShowIp { get; set; }
    }
}
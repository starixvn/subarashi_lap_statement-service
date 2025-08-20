namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class BetListByProductViewModel : BaseViewModel
    {
        public string ProductCode { get; set; }

        public string TransDesc { get; set; }

        public IList<TicketText> Data { get; set; }

        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TransType { get; set; }

        public byte SortType { get; set; }

        public byte SortOrder { get; set; }

        public DateTime WinlostDate { get; set; }

        public int TotalRows { get; set; }

        public bool IsDesktopViewOnly { get; set; }
    }
}
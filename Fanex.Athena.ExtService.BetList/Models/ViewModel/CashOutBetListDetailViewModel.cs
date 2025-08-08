namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class CashOutBetListDetailViewModel : BaseViewModel
    {
        public TicketText OriginalTicketText { get; set; }

        public IEnumerable<CashOutTicket> CashOutTicketList { get; set; }

        public IEnumerable<CashOutTicketText> CashOutTicketTextList { get; set; }

        public bool IsCashOut { get; set; }

        public bool IsHighlightOriginalTicket { get; set; }
    }
}
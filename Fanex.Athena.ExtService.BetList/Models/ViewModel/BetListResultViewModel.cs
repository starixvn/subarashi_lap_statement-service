namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class BetListResultViewModel : BaseViewModel
    {
        public List<TicketText> Main { get; set; }

        public BetListTotalData Total { get; set; }

        public string LeagueName { get; set; }

        public bool HasTotalRow { get; set; }

        public bool HasLicWl { get; set; }

        public bool IsDesktopViewOnly { get; set; }
    }
}
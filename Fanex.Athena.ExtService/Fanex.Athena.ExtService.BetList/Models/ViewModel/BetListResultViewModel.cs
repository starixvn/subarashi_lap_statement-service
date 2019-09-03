using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Shared.DTO;
using Fanex.Athena.ExtService.Shared.Model.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class BetListResultViewModel : BaseViewModel
    {
        public List<TicketText> Main { get; set; }

        public BetListTotalData Total { get; set; }

        public string LeagueName { get; set; }

        public string BaseCurrency { get; set; }

        public LicenseeInfo Info { get; set; }

        public bool HasTotalRow { get; set; }
        public bool HasLicWl { get; set; }
    }
}

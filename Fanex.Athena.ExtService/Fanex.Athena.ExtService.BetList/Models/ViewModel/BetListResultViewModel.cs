using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.DTO;
using Fanex.Athena.ExtService.Common.Model.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class BetListResultViewModel : BaseViewModel
    {
        public List<TicketText> Main { get; set; }

        public BetListTotalData Total { get; set; }

        public string LeagueName { get; set; }

        public bool HasTotalRow { get; set; }
        public bool HasLicWl { get; set; }
    }
}

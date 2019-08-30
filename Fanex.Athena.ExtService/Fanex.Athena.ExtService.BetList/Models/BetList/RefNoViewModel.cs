using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.Shared.Model.DTO;
using Fanex.Athena.ExtService.Shared.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.BetList
{
    public class RefNoViewModel : BaseViewModel
    {
        public string BaseCurrency { get; set; }
        public string UserName { get; set; }
        public BalanceUp BalanceUp { get; set; }

        public IList<TicketText> Data { get; set; }
    }
}

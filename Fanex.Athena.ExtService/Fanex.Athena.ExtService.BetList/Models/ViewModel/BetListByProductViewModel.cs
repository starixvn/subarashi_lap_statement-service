using System.Collections.Generic;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    public class BetListByProductViewModel : BaseViewModel
    {
        public string ProductCode { get; set; }

        public string TransDesc { get; set; }

        public IList<TicketText> Data { get; set; }
    }
}

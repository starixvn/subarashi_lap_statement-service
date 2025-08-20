namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class TxIDViewModel : BaseViewModel
    {
        public string UserName { get; set; }

        public IList<TicketText> Data { get; set; }
    }
}
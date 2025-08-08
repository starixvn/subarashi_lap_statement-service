namespace Fanex.Athena.ExtService.BetList.Models.ViewModel
{
    using System.Collections.Generic;
    using Common.Models.ViewModels;
    using DTO;

    public class StatementByGroupViewModel : BaseViewModel
    {
        public IEnumerable<StatementByGroup> DataList { get; internal set; }

        public Dictionary<string, string> ProductResources { get; set; }

        public string TransDesc { get; set; }

        public int CustId { get; set; }

        public int ModeView { get; set; }

        public bool IsDesktopViewOnly { get; set; }
    }
}
namespace Fanex.Athena.ExtService.Report.Models.MetaModel
{
    using Common.Models.DTO;

    public interface IBaseReportMetaModel
    {
        public string Date { get; set; }

        public string AgentListXml { get; set; }

        public int SpecialPlatform { get; set; }

        public BalanceUp BalanceUp { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
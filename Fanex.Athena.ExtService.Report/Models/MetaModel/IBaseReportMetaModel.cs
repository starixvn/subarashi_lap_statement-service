namespace Fanex.Athena.ExtService.Report.Models.MetaModel
{
    public interface IBaseReportMetaModel
    {
        public string Date { get; set; }

        public string AgentListXml { get; set; }

        public int SpecialPlatform { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
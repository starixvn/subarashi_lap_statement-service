namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public interface IBaseBetListMetaModel : IBaseMetaModel
    {
        bool HasPlayerCom { get; set; }

        int SpecialPlatform { get; set; }

        string AgentListXml { get; set; }

        bool IsShowIpvn { get; set; }
    }
}
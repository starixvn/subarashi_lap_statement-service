namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    using System;
    using Common.Models.DataDomain;
    using Common.Models.ViewModels;

    public class BetListDetailByProductMetaModel : IBaseBetListMetaModel, ICustomerMetaModel, IMetaModelShowIP
    {
        public long TransId { get; set; }

        public long TransType { get; set; }

        public int CustId { get; set; }

        public DateTime Date { get; set; }

        public string UspKey { get; set; }

        public bool HasPlayerCom { get; set; }

        public bool HasTotal { get; set; }

        public bool HasLicWl { get; set; }

        public long RefCode { get; set; }

        public string AgentListXml { get; set; }

        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;

        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsDesktopViewOnly { get; set; }

        public bool IsMobileView { get; set; }

        public bool IsShowIpvn { get; set; }

        public bool IsShowIp { get; set; }

        public int SpecialPlatform { get; set; }
    }
}
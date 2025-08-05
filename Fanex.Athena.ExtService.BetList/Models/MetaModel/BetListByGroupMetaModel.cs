namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System.Linq;
    using Core.Helper;
using Fanex.Athena.ExtService.Common.Constant;
    using Microsoft.AspNetCore.Mvc;
    using Models.DataDomain;
    using Models.MetaModel;

    public class BetListByGroupMetaModel : StatementByGroupMetaModel, IBaseBetListMetaModel, IMetaModelShowIP
    {
        public new bool IsValid
        {
            get
            {
                var pagingValidation = PageIndex > 0 && PageSize > 0;
                var mainValidation = CustId > 0 && WinlostDate > DateTimeHelper.MinValue;
                var productAndModeValidation =
                    !string.IsNullOrEmpty(ProductCode) && (TransType == 0 || TransDescValue.MaptoModeView.Values.Contains(TransType));
                return pagingValidation && mainValidation && productAndModeValidation;
            }
        }

        [FromQuery(Name = "pc")]
        public string ProductCode { get; set; }

        [FromQuery(Name = "st")]
        public byte SortType { get; set; } = (byte)SortTypes.TransDate;

        [FromQuery(Name = "so")]
        public byte SortOrder { get; set; } = (byte)SortOrders.Ascending;

        [FromQuery(Name = "spi")]
        public int StatementPageIndex { get; set; }

        [FromQuery(Name = "sps")]
        public int StatementPageSize { get; set; }

        public int TransType
        {
            get { return ModeView; }
        }

        public bool HasPlayerCom { get; set; }

        public string AgentListXml { get; set; }

        public bool IsShowIpvn { get; set; }

        public int SpecialPlatform { get; set; }

        public bool IsShowIp { get; set; }
    }
}
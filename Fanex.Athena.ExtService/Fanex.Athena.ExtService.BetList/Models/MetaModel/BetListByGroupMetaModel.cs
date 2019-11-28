using System;
using System.Collections.Generic;

using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    public class BetListByGroupMetaModel : IBaseBetListMetaModel, ICustomerMetaModel
    {
        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;
        public bool IsValid
        {
            get
            {
                var pagingValidation = PageIndex > 0 && PageSize > 0;
                var mainValidation = CustId > 0 && Date > DateTime.MinValue;
                var productAndModeValidation = !string.IsNullOrEmpty(ProductCode) && (TransType == 0 || TransDescValue.MaptoModeView.Values.Contains(TransType));
                return pagingValidation && mainValidation && productAndModeValidation;
            }
        }

        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = SystemConstants.DefaultPageSize;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "wd")]
        public DateTime Date { get; set; }
        [FromQuery(Name = "pc")]
        public string ProductCode { get; set; }

        [FromQuery(Name = "s")]
        public string Sort { get; set; } = "Transid ASC";

        [FromQuery(Name = "mv")]
        public int TransType { get; set; }

        public bool HasPlayerCom { get; set; }
        public string AgentListXml { get; set; }

    }
}
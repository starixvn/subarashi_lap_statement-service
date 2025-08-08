namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    using System;

    public class NewCashOutBetListDetailMetaModel : CashOutBetListDetailMetaModel
    {
        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }
    }
}
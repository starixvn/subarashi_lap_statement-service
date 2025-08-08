namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    using System;

    public class SummaryStatement : IStatementValue
    {
        public int RowNum { get; set; }

        public DateTime WinLostDate { get; set; }

        public string TransType { get; set; }

        public string TransDesc { get; set; }

        public Decimal TurnOver { get; set; }

        public Decimal TotalAmt { get; set; }

        public Decimal Commission { get; set; }

        public int BetTypeId { get; set; }
    }
}
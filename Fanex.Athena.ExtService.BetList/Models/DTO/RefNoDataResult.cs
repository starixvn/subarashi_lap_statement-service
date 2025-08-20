namespace Fanex.Athena.ExtService.BetList.Models.DTO
{
    using System.Collections.Generic;

    public class RefNoDataResult
    {
        public IList<TicketText> Data { get; set; }

        public int TotalRows { get; set; }
    }
}
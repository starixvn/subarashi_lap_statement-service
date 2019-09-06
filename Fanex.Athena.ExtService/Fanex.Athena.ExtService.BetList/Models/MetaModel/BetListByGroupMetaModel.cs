using Fanex.Athena.ExtService.BetList.Models.MetaModel;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    public class BetListByGroupMetaModel : IBaseMetaModel
    {
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
        public bool IsValid => true;

    }
}
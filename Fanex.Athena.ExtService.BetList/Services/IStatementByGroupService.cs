namespace Fanex.Athena.ExtService.BetList.Services
{
    using System.Threading.Tasks;
    using Models.MetaModel;
    using Models.ViewModel;

    public interface IStatementByGroupService
    {
        Task<StatementByGroupViewModel> QueryAsync(StatementByGroupMetaModel metaModel);
    }
}
namespace Fanex.Athena.ExtService.Common.Services
{
    using RemotingRef;

    public interface ICacheRefAdapter
    {
        IRemotingRefService RemotingRefService { get; }

        string GetBetTypeName(object betTypeId);
    }
}
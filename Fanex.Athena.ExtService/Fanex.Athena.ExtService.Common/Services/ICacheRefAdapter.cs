namespace Fanex.Athena.ExtService.Common.Services
{
    using Fanex.RemotingRef;

    public interface ICacheRefAdapter
    {
        IRemotingRefService RemotingRefService { get; }

        string GetBetTypeName(object betTypeId);
    }
}

namespace Fanex.Athena.ExtService.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IAuthenticateService
    {
        Task<Tuple<bool, Token>> IsAuthenticated(TokenRequest request);
    }
}
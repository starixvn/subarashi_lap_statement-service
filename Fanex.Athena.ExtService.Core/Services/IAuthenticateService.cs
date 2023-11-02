using Fanex.Athena.ExtService.Core.Models;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IAuthenticateService
    {
        Task<Tuple<bool, Token>> IsAuthenticated(TokenRequest request);
    }
}

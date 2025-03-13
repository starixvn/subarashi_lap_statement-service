using Fanex.Athena.ExtService.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IOAuthService
    {
        Task<string> RequestTokenAsync(TokenRequest request);

        Task<IEnumerable<Claim>> GetUserInfo(string accessToken);
    }
}

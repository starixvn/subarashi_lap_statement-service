namespace Fanex.Athena.ExtService.Core.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Models;

    public interface IOAuthService
    {
        Task<string> RequestTokenAsync(TokenRequest request);

        Task<IEnumerable<Claim>> GetUserInfo(string accessToken);
    }
}

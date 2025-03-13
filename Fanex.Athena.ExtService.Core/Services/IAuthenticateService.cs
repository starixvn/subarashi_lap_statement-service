using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Core.Models;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IAuthenticateService
    {
        Task<Tuple<bool, Token>> IsAuthenticated(TokenRequest request);
    }
}
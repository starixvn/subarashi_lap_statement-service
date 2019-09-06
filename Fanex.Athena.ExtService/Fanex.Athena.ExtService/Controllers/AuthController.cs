using Fanex.Athena.ExtService.Core.Attributes;
using Fanex.Athena.ExtService.Core.Models;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(IPFilterAttribute))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthController(IAuthenticateService authService)
        {
            _authService = authService;
        }
        [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.None)]
        [AllowAnonymous]
        [Route("api/Auth")]
        [HttpPost]
        public ActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            return _authService.IsAuthenticated(request, out Token token) ? Ok(token)
                : (ActionResult)BadRequest("Wrong username or password");
        }
    }
}
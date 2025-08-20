namespace Fanex.Athena.ExtService.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ServiceFilter(typeof(IPFilterAttribute))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [Route("api/auth")]
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            var result = await _authService.IsAuthenticated(request);

            return result.Item1 ? Ok(result.Item2)
                : BadRequest("Wrong username or password");
        }
    }
}
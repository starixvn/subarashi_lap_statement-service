using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class BaseControllerTest
    {
        protected readonly Mock<HttpContext> _httpContext;
        protected readonly Mock<IIdentity> _identity;
        protected readonly Mock<ControllerContext> _controllerContext;
        protected const string SiteName = "12BET";
        protected string AccessToken = "ANy";
        public BaseControllerTest()
        {

            _identity = new Mock<IIdentity>();
            _identity.Setup(x => x.IsAuthenticated).Returns(true);
            _identity.Setup(x => x.Name).Returns(SiteName);

            _httpContext = new Mock<HttpContext>();
            _httpContext.Setup(x => x.Request.Query["access_token"]).Returns(AccessToken);
            var claimsPrinciple = new ClaimsPrincipal(_identity.Object);
            _httpContext.Setup(x => x.User).Returns(claimsPrinciple);

            _controllerContext = new Mock<ControllerContext>();
            _controllerContext.Object.HttpContext = _httpContext.Object;
        }
    }
}

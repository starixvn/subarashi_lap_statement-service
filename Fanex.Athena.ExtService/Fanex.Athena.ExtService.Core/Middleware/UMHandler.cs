using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ONEbook.UM.Services;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class UMHandler
    {
        private readonly RequestDelegate _next;
        private readonly IMaintenanceService _maintenanceService;
        private const string UMPage = "/Error/UM";
        public UMHandler(RequestDelegate next, IMaintenanceService maintenanceService)
        {
            _next = next;
            _maintenanceService = maintenanceService;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.Contains(UMPage, StringComparison.OrdinalIgnoreCase))
            {

            }
            await _next.Invoke(context);
        }

    }
}

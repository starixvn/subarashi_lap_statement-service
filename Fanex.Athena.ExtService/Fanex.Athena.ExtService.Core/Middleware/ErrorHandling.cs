using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;
        public ErrorHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            if (error != null && error.Error != null)
            {
                await HandleError(context, error.Error);
            }

            try
            {
                await _next.Invoke(context);

            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private static async Task HandleError(HttpContext context, Exception error)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            Logging.Logger.Log.Error(error.Message, error);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = error.Message
            }));
        }
    }


}

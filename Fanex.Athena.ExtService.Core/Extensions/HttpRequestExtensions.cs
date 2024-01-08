using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Fanex.Athena.ExtService.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private static readonly string HostName = Dns.GetHostName();

        public static bool FromInternal(this HttpRequest request)
            => HostName.Contains(request.Host.Host, StringComparison.OrdinalIgnoreCase);
    }
}
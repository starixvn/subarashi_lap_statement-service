namespace Fanex.Athena.ExtService.Core.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Services;
    using Extensions;
    using Helper;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Newtonsoft.Json;

    public class RateLimitHandler
    {
        private const int CacheTimeout300Seconds = 300;
        private readonly ICustomDistributedCache _cache;
        private readonly RequestDelegate _next;

        public RateLimitHandler(
            RequestDelegate next,
            ICustomDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.TryGetRateLimitAttribute(out var decorator))
            {
                await _next(context);
                return;
            }

            var consumptionData = await _cache.GetAsync<ConsumptionData>(context.GetCustomerKey());

            if (consumptionData is not null)
            {
                if (consumptionData.HasConsumedAllRequests(decorator!.TimeWindowInSeconds, decorator!.MaxRequests))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        State = "Too Many Requests",
                        Msg = "Too many requests hit the API too quickly. We recommend an exponential backoff of your requests."
                    }));
                    return;
                }

                consumptionData.IncreaseRequests(decorator!.MaxRequests);
            }
            else
            {
                consumptionData = new ConsumptionData(DateTimeHelper.UtcNow, 1);
            }

            await _cache.SetAsync(
                context.GetCustomerKey(),
                consumptionData,
                CustomDistributedCache.WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds)));

            await _next(context);
        }
    }
}
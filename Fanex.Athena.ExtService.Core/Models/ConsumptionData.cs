namespace Fanex.Athena.ExtService.Core.Models
{
    using System;
    using Helper;

    public class ConsumptionData
    {
        public ConsumptionData(
            DateTime lastResponse,
            int numberOfRequests)
        {
            LastResponse = lastResponse;
            NumberOfRequests = numberOfRequests;
        }

        public DateTime LastResponse { get; private set; }

        public int NumberOfRequests { get; private set; }

        public bool HasConsumedAllRequests(int timeWindowInSeconds, int maxRequests)
            => DateTimeHelper.UtcNow < LastResponse.AddSeconds(timeWindowInSeconds) && NumberOfRequests == maxRequests;

        public void IncreaseRequests(int maxRequests)
        {
            if (NumberOfRequests == maxRequests)
            {
                LastResponse = DateTimeHelper.UtcNow;
                NumberOfRequests = 1;
            }
            else
            {
                NumberOfRequests++;
            }
        }
    }
}
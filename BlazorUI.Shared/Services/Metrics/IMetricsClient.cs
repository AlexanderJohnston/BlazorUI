using System.Collections.Generic;

namespace BlazorUI.Shared.Services.Metrics
{
    public interface IMetricsClient
    {
        public void TrackEvent(string method, Dictionary<string, double> metrics);
    }
}

using BlazorUI.Shared.Events.Database;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries
{
    public class BatchStatusQuery : Query
    {
        public double PercentProgress = 0;

        void Given(BatchStatusUpdated e)
        {
            PercentProgress = e.PercentProgress;
        }
    }
}

using BlazorUI.Shared.Events.Database;
using System;
using System.Collections.Generic;
using System.Text;
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

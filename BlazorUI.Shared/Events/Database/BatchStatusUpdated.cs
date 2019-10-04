using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Database
{
    public class BatchStatusUpdated : Event
    {
        public readonly double PercentProgress;

        public BatchStatusUpdated(float percentProgress)
        {
            PercentProgress = percentProgress;
        }
    }
}

using BlazorUI.Shared.Data;
using System.Collections.Generic;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Database
{
    public class LegacyEventsQueried : Totem.Timeline.Event
    {
        public readonly List<string> Events;

        public LegacyEventsQueried(List<string> events)
        {
            Events = events;
        }
    }
}
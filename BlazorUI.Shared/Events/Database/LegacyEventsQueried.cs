using BlazorUI.Shared.Data;
using System.Collections.Generic;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Database
{
    public class LegacyEventsQueried : Event
    {
        public readonly List<LegacyEvent> Events;

        public LegacyEventsQueried(List<LegacyEvent> events)
        {
            Events = events;
        }
    }
}
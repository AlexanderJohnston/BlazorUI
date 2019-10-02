using System.Collections.Generic;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Database
{
    public class LegacyEventsQueried : Event
    {
        public readonly List<string> Events;

        public LegacyEventsQueried(List<string> events)
        {
            Events = events;
        }
    }
}
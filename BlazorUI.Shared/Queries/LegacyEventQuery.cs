using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events.Database;
using System.Collections.Generic;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries
{
    public class LegacyEventQuery : Query
    {
        public List<LegacyEvent> Events = new List<LegacyEvent>();

        void Given(LegacyEventsQueried e)
        {
            Events = e.Events;
        }
    }
}

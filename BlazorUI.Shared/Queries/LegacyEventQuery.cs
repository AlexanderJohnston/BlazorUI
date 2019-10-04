using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries
{
    public class LegacyEventQuery : Query
    {
        public List<LegacyEvent> Events = new List<LegacyEvent>();

        public void Given(LegacyEventsQueried e)
        {
            Events = e.Events;
        }
    }
}

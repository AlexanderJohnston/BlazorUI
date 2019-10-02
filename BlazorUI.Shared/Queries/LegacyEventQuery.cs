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
        public List<string> Events { get; set; } = new List<string>();

        void Given(LegacyEventsQueried e)
        {
            Events = e.Events;
        }
    }
}

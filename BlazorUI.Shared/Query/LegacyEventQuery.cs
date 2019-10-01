using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorUI.Shared.Query
{
    public class LegacyEventQuery : Totem.Timeline.Query
    {
        public List<string> Events { get; set; } = new List<string>();

        void Given(LegacyEventsQueried e)
        {
            Events = e.Events;
        }
    }
}

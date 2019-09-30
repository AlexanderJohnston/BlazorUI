using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Shared.Topic
{
    public class Database : Totem.Timeline.Topic
    {
        async Task When(QueryEvents e, ILegacyEventContext context)
        {
            var events = await context.GetEvents();
            var safeEvents = new List<string>();
            foreach (var ev in events)
            {
                safeEvents.Add(JsonConvert.SerializeObject(new LegacyEvent(ev)));
            }
            Then(new LegacyEventsQueried(safeEvents));
        }

        void When(LegacyEventsQueried e)
        {
            Then(new LegacyEventsSucceeded());
        }
    }
}

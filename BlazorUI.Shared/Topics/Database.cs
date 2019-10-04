using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    public class Database : Topic
    {
        public async Task When(QueryEvents e, ILegacyEventContext context)
        {
            var events = await context.GetEvents();
            var safeEvents = events.Select(ev => new LegacyEvent(ev)).ToList();
            //var safeEvents = new List<string>();
            //foreach (var ev in events)
            //{
            //    safeEvents.Add(JsonConvert.SerializeObject(new LegacyEvent(ev)));
            //}
            Then(new LegacyEventsQueried(safeEvents));
        }

        public void When(LegacyEventsQueried e)
        {
            Then(new LegacyEventsSucceeded());
        }
    }
}

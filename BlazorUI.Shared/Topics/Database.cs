using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events.Database;
using BlazorUI.Shared.Services.Aspect;
using PostSharp.Patterns.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    //[Profile]
    public class Database : Topic
    {
        private readonly int BatchProcessingSize = 10;

        [EntryPoint]
        async Task When(QueryEvents e, ILegacyEventContext context)
        {
            bool singleStep = e.Count == BatchProcessingSize;
            bool underSized = e.Count < BatchProcessingSize;
            var numberOfSteps = singleStep ? 1
                : underSized ? 1
                : (e.Count / BatchProcessingSize);
            int remainder = e.Count % BatchProcessingSize;
            int checkpoint = 0;
            List<TotemV1Event> events = new List<TotemV1Event>();
            for (int step = 1; step <= numberOfSteps; step++)
            {
                events.AddRange(await context.GetEvents(BatchProcessingSize, checkpoint));
                checkpoint = checkpoint + BatchProcessingSize;
                Then(new BatchStatusUpdated((100 / (float)numberOfSteps) * step));
            }
            if (remainder > 0)
            {
                events.AddRange(await context.GetEvents(remainder, checkpoint));
            }
            var safeEvents = events.Select(ev => new LegacyEvent(ev)).ToList();
            //var safeEvents = new List<string>();
            //foreach (var ev in events)
            //{
            //    safeEvents.Add(JsonConvert.SerializeObject(new LegacyEvent(ev)));
            //}
            Then(new LegacyEventsQueried(safeEvents));
        }

        void When(LegacyEventsQueried e)
        {
            Then(new LegacyEventsSucceeded());
        }
    }
}

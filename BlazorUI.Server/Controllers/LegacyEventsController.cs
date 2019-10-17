using BlazorUI.Client.Pages.Data;
using BlazorUI.Server.Attributes;
using BlazorUI.Shared.Events.Database;
using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    [Route("[controller]")]
    public class LegacyEventsController : Controller
    {
        private IQueryServer _queries { get; set; }
        private ICommandServer _commands { get; set; }

        public LegacyEventsController(IQueryServer queries, ICommandServer commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpPost("[action]")]
        public Task<IActionResult> FetchEvents([FromBody] string batchSize)
        {
            var batch = JsonConvert.DeserializeObject<BatchSize>(batchSize);
            return _commands.Execute(new QueryEvents(batch.Count), When<LegacyEventsSucceeded>.ThenOk, When<LegacyEventsFailed>.ThenBadRequest);
        }

        [HttpGet("[action]")]
        [TimelineQuery(typeof(LegacyEventQuery))]
        public Task<IActionResult> Get() => _queries.Get<LegacyEventQuery>();

        [HttpGet("[action]")]
        [TimelineQuery(typeof(BatchStatusQuery))]
        public Task<IActionResult> Progress() => _queries.Get<BatchStatusQuery>();
    }
}

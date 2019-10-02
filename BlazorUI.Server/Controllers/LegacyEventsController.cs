using BlazorUI.Server.Attributes;
using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Database;
using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<IActionResult> FetchEvents() => _commands.Execute(new QueryEvents(),
            When<LegacyEventsSucceeded>.ThenOk, When<LegacyEventsFailed>.ThenBadRequest);

        [HttpGet("[action]")]
        [TimelineQuery(typeof(LegacyEventQuery))]
        public Task<IActionResult> Get() => _queries.Get<LegacyEventQuery>();
    }
}

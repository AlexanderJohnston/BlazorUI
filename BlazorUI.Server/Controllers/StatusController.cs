using System.Threading.Tasks;
using BlazorUI.Server.Attributes;
using DealerOn.Cam;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the API root
    /// </summary>
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private IQueryServer _queries { get; set; }
        private ICommandServer _commands { get; set; }

        public StatusController(IQueryServer queries, ICommandServer commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet("[action]")]
        public Task<IActionResult> GetStatus() => _queries.Get<StatusQuery>();

        [HttpGet("[action]")]
        [TimelineQuery(typeof(EchoQuery))]
        public Task<IActionResult> GetEcho() => _queries.Get<EchoQuery>();

        [HttpPost("[action]")]
        public Task<IActionResult> SendEcho() => _commands.Execute(new Echo(), 
            When<EchoSuccess>.ThenOk, When<EchoFailure>.ThenBadRequest);
    }
}
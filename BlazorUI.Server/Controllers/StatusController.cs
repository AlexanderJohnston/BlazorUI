using System.Threading.Tasks;
using DealerOn.Cam;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the API root
    /// </summary>
    public class StatusController : Controller
    {
        [HttpGet("[action]")]
        public Task<IActionResult> GetStatus([FromServices] IQueryServer queries) => queries.Get<StatusQuery>();

        [HttpGet("[action]")]
        public Task<IActionResult> GetEcho([FromServices] IQueryServer queries) => queries.Get<EchoQuery>();

        [HttpPost("[action]")]
        public Task<IActionResult> SendEcho([FromServices] ICommandServer commands) => commands.Execute(new Echo(), When<EchoSuccess>.ThenOk, When<EchoFailure>.ThenBadRequest);
    }
}
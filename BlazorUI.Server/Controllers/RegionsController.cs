using System.Threading.Tasks;
using BlazorUI.Client.Campaign;
using BlazorUI.Client.Campaign.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the set of regions
    /// </summary>
    public class RegionsController : Controller
  {
    [HttpGet("/api/regions")]
    public Task<IActionResult> GetStatus([FromServices] IQueryServer queries) =>
      queries.Get<RegionsQuery>();

        [HttpPost("/api/regions/fake")]
        public Task<IActionResult> PostStatus([FromServices] ICommandServer commands) =>
                commands.Execute(new FakeManifest(), new CommandWhen(typeof(string),FakeWhenOk), new CommandWhen(typeof(string),FakeWhenBad));

        public IActionResult FakeWhenOk(Event e)
        {
            return (IActionResult)( new OkResult());
        }
        public IActionResult FakeWhenBad(Event e)
        {
            return (IActionResult)(new OkResult());
        }
    }
}
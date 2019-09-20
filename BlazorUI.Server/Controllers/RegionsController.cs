using System.Threading.Tasks;
using BlazorUI.Server.Attributes;
using DealerOn.Cam;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the set of regions
    /// </summary>
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private IQueryServer _queries { get; set; }
        private ICommandServer _commands { get; set; }
        public RegionsController(IQueryServer queries, ICommandServer commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet("[action]")]
        public Task<IActionResult> GetStatus() => _queries.Get<RegionsQuery>();
    
        [HttpPost("[action]")]
        public Task<IActionResult> PostStatus() => _commands.Execute(new FakeManifest(), 
            When<FakedManifest>.ThenOk, When<FakedBad>.ThenBadRequest);
    }
}
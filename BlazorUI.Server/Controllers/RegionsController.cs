using System.Threading.Tasks;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
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
  }
}
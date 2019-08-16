using System.Threading.Tasks;
using BlazorUI.Client.Campaign.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
  /// <summary>
  /// Controls interactions with the set of regions
  /// </summary>
  public class DealerDetailsController : Controller
  {
    [HttpGet("/api/dealers/{id}")]
    public Task<IActionResult> GetStatus([FromRoute] Id id, [FromServices] IQueryServer queries) =>
      queries.Get<DealerDetailsQuery>(id);
  }
}
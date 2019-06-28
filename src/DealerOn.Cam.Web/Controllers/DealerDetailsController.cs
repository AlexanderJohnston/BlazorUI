using System.Threading.Tasks;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem;
using Totem.Timeline.Mvc;

namespace DealerOn.Cam.Service.Controllers
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
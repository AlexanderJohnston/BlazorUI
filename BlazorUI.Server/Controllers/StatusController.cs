using System.Threading.Tasks;
using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace DealerOn.Cam.Service.Controllers
{
  /// <summary>
  /// Controls interactions with the API root
  /// </summary>
  public class StatusController : Controller
  {
    [HttpGet("/api")]
    public Task<IActionResult> GetStatus([FromServices] IQueryServer queries) =>
      queries.Get<StatusQuery>();
  }
}
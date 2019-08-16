using System.Threading.Tasks;
using BlazorUI.Client.Campaign.Queries;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
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
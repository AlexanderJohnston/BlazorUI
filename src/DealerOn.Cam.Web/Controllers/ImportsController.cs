using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace DealerOn.Cam.Service.Controllers
{
  /// <summary>
  /// Controls interactions with the set of imports
  /// </summary>
  public class ImportsController : Controller
  {
    [HttpPost("/api/imports")]
    public Task<IActionResult> StartImport([FromServices] ICommandServer commands) =>
      commands.Execute(
        new StartImport(),
        When<ImportStarted>.ThenOk,
        When<ImportAlreadyStarted>.ThenConflict);
  }
}
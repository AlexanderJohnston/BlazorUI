using Microsoft.AspNetCore.Mvc;

namespace DealerOn.Cam.Web.Controllers
{
  /// <summary>
  /// Controls interactions with the application root
  /// </summary>
  public class AppController : Controller
  {
    [HttpGet("/")]
    public IActionResult Home() =>
      View("Views/App.cshtml");
  }
}
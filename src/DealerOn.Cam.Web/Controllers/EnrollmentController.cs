using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Microsoft.AspNetCore.Mvc;
using Totem;
using Totem.Timeline.Mvc;

namespace DealerOn.Cam.Web.Controllers
{
  /// <summary>
  /// Controls interactions with the set of enrolled dealers
  /// </summary>
  public class EnrollmentController : Controller
  {
    [HttpPut("/api/enrollment/{pages}/{dealerId}")]
    public async Task<IActionResult> ChangeEnrollment([FromServices] ICommandServer commands, string pages, Id dealerId)
    {
      if(!TryParsePages(pages, out var parsedPages))
      {
        return new NotFoundResult();
      }

      return await commands.Execute(
        new ChangeEnrollment(dealerId, parsedPages),
        When<DealerNotEnrolled>.ThenNotFound,
        When<EnrollmentChanged>.ThenOk,
        When<EnrollmentUnchanged>.ThenOk);
    }

    bool TryParsePages(string value, out Pages pages)
    {
      switch(value)
      {
        case "none":
          pages = Pages.None;
          return true;
        case "home":
          pages = Pages.Home;
          return true;
        case "conditional":
          pages = Pages.Conditional;
          return true;
        case "all":
          pages = Pages.All;
          return true;
        default:
          pages = default;
          return false;
      }
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Shared;
using DealerOn.Cam;
using Microsoft.AspNetCore.Mvc;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    /// <summary>
    /// Controls interactions with the set of imports
    /// </summary>
    [Route("api/[controller]")]
    public class ImportsController : Controller
    {
        [HttpPost("[action]")]
        public Task<IActionResult> StartImport([FromServices] ICommandServer commands) =>
            commands.Execute(
                new StartImport(),
                When<ImportStarted>.ThenOk,
                When<ImportAlreadyStarted>.ThenConflict);

        [HttpGet("[action]")]
        public string Test() => "test";
    }
}
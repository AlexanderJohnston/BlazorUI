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
    [Route("[controller]")]
    public class ImportsController : Controller
    {
        private ICommandServer _commands { get; set; }
        public ImportsController(ICommandServer commands)
        {
            _commands = commands;
        }

        [HttpPost("[action]")]
        public Task<IActionResult> StartImport() =>
            _commands.Execute(
                new StartImport(),
                When<ImportStarted>.ThenOk, 
                When<ImportAlreadyStarted>.ThenConflict);

        [HttpGet("/test")]
        public string Test() => "test";
    }
}
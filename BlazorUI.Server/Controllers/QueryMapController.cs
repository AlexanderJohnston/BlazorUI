using BlazorUI.Client.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Server.Controllers
{
    [Route("[controller]")]
    public class QueryMapController
    {
        private IRouteContext _routes { get; set; }

        public QueryMapController(IRouteContext routes)
        {
            _routes = routes;
        }

        [HttpGet("[action]")]
        public string Get() => JsonConvert.SerializeObject(_routes.Map);
    }
}

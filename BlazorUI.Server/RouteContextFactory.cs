using BlazorUI.Client.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Server
{
    public class RouteContextFactory : IRouteContextFactory
    {
        private Func<IRouteContext> _contextCreator;

        public IRouteContext CreateContext() => _contextCreator();

        public RouteContextFactory(Func<IRouteContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
    }
}

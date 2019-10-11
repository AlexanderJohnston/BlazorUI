using BlazorUI.Client.Queries;
using System;

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

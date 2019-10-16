using BlazorUI.Client.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Server
{
    /// <summary>
    ///     Originally intended to inject the context into the client as a Transient service.
    ///     Evaluation of the context can't be done from the client but it was being lazily
    ///     evaluated that way due to the dependency injection. The server would correctly
    ///     generate the list of timeline routes, but the client wouldn't ask for that list
    ///     until it was too late. It was a real head scratcher and this was during 3.0Preview7.
    /// </summary>
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

using BlazorUI.Client.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public interface IRouteContextFactory
    {
        IRouteContext CreateContext();
    }
}

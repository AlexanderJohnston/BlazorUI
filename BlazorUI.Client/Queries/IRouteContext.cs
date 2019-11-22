using System.Collections.Generic;

namespace BlazorUI.Client.Queries
{
    public interface IRouteContext
    {
        List<TimelineRoute> Map { get; set; }
    }
}

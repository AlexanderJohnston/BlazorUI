using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Client.totem_timeline;

namespace BlazorUI.Client.totem_timeline_signalr
{
    public class QueryHub
    {
        public dynamic Connection = null;

        public dynamic ConnectionStart = null;

        public void Enable(object url, object configure)
        {
            //var onChanged = Timeline.ConfigureQueryHub()
        }
    }
}

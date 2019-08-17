using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.totem_timeline_signalr
{
    public class Query
    {
        public string Type { get; set; }
        public delegate void QueryBinding(string response);
        public QueryBinding ReadQuery { get; set; }
        public Func<string, Task> ReadQueryAsync { get; set; }

    }
}

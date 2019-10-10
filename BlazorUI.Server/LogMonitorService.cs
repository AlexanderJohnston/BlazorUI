using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Microsoft;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Runtime;

namespace BlazorUI.Server
{
    public class LogMonitorService : Notion
    {
        public void Test()
        {
            var log = Notion.Traits.Log.ResolveDefaultTyped(this);
        }
    }
}

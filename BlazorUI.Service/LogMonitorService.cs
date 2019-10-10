using System;
using System.Collections.Generic;
using System.Text;
using Totem.Runtime;

namespace BlazorUI.Service
{
    public class LogMonitorService : Notion
    {
        public void Test()
        {
            var log = Notion.Traits.Log.ResolveDefaultTyped(this);
        }
    }
}

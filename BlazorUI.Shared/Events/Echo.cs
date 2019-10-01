using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Events
{
    public class Echo : Command
    {
        public readonly DateTime? Instantiated;

        public Echo()
        {
            Instantiated = DateTime.Now;
        }
    }

    public class Echoed : Totem.Timeline.Event 
    {
        public readonly DateTime? Instantiated;

        public Echoed()
        {
            Instantiated = DateTime.Now;
        }
    }

    public class EchoSuccess : Totem.Timeline.Event { public readonly DateTime? Instantiated; public EchoSuccess() { Instantiated = DateTime.Now; } }

    public class EchoFailure : Totem.Timeline.Event { public readonly DateTime? Instantiated; public EchoFailure() { Instantiated = DateTime.Now; } }
}

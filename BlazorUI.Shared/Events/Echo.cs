﻿using System;
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

    public class Echoed : Event
    {
        public readonly DateTime? Instantiated;

        public Echoed()
        {
            Instantiated = DateTime.Now;
        }
    }

    public class EchoSuccess : Event { public readonly DateTime? Instantiated; public EchoSuccess() { Instantiated = DateTime.Now; } }

    public class EchoFailure : Event { public readonly DateTime? Instantiated; public EchoFailure() { Instantiated = DateTime.Now; } }
}

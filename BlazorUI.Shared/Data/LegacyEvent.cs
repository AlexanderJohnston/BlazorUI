using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorUI.Shared.Data
{
    public class LegacyEvent
    {
        public readonly long Position;
        public readonly long Cause;
        public readonly string Type;
        public readonly string Json;
        
        public LegacyEvent()
        {

        }
        public LegacyEvent(TotemV1Event e)
        {
            Position = e.Position ?? 0;
            Cause = e.Cause ?? 0;
            Type = e.Type;
            Json = e.Json;
        }
    }
}

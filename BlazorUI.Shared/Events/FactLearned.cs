using BlazorUI.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Runtime;
using Totem.Timeline;

namespace BlazorUI.Shared.Events
{
    class FactLearned : Event
    {
        public Moment Happened { get; set; }
        public string Knowledge { get; set; }
        public Dictionary<string, string> Fields { get; set; }

        public FactLearned(Moment happened, string knowledge, Dictionary<string, string> fields)
        {
            Happened = happened;
            Knowledge = knowledge;
            Fields = fields;
        }
    }
}

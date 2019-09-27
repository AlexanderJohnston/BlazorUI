using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Runtime;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries
{
    class Fact : Query
    {
        static Many<Id> RouteFirst(FactLearned e) => e.Happened.Position;
        public Moment Happened { get; set; }
        public string Knowledge { get; set; }
        public Dictionary<string, string> Fields { get; set; }

        void Given(FactLearned e)
        {
            Happened = e.Happened;
            Knowledge = e.Knowledge;
            Fields = e.Fields;
        }
    }
}

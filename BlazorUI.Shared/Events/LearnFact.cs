using BlazorUI.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Totem;

namespace BlazorUI.Shared.Events
{
    public class LearnFact
    {
        public Moment Happened { get; set; }
        public string Knowledge { get; set; }

        public LearnFact(Moment happened, string json)
        {
            Happened = happened;
            Knowledge = json;
        }
    }
}

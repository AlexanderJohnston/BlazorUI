using BlazorUI.Shared.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    public class Listener : Topic
    {
        public DateTime? LastEntered;
        public int EnterCount;

        void Given (Echoed e)
        {
            LastEntered = DateTime.Now;
            EnterCount++;
        }

        void When(Echo e) 
        {
            Log.LogInformation("Testing echo logger.");
            Then(new Echoed()); 
        }

        void When(Echoed e)
        {
            Then(new EchoSuccess());
        }
    }
}

using BlazorUI.Shared.Events;
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

        public Listener()
        {
            LastEntered = DateTime.Now;
        }

        void Given (Echoed e)
        {
            LastEntered = DateTime.Now;
            EnterCount++;
        }

        void When(Echo e) 
        { 
            Then(new Echoed()); 
        }

        void When(Echoed e)
        {
            Then(new EchoSuccess());
        }
    }
}

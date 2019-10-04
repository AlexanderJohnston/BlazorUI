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

        public void Given (Echoed e)
        {
            LastEntered = DateTime.Now;
            EnterCount++;
        }

        public void When(Echo e) 
        { 
            Then(new Echoed()); 
        }

        public void When(Echoed e)
        {
            Then(new EchoSuccess());
        }
    }
}

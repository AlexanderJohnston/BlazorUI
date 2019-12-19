using BlazorUI.Shared.Events;
using BlazorUI.Shared.Events.Game;
using Microsoft.Extensions.Logging;
using System;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    //[Profile]
    public class Listener : Topic
    {
        public DateTime? LastEntered;
        public int EnterCount;

        void Given(Echoed e)
        {
            LastEntered = DateTime.Now;
            EnterCount++;
        }

        void When(Echo e)
        {
            Log.LogInformation("Testing echo logger.");
            Then(new CreateLobby("Random", "root"));
            Then(new Echoed());
        }

        void When(Echoed e)
        {
            Then(new EchoSuccess());
        }
    }
}

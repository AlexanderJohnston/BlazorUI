using BlazorUI.Shared.Data;
using BlazorUI.Shared.Events.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Shared.Topic
{
    public class Chat : Totem.Timeline.Topic
    {
        public Chat()
        {

        }

        async Task When(SendMessage e)
        {
            Then(new MessageReceived(e.Message, e.User));
        }
        void When(MessageReceived e)
        {
            Then(new MessageSucceeded());
        }
    }
}

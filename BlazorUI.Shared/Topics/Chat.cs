using BlazorUI.Shared.Events.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    public class Chat : Topic
    {
        public Chat()
        {

        }

        public async Task When(SendMessage e)
        {
            Then(new MessageReceived(e.Message, e.User));
        }
        public void When(MessageReceived e)
        {
            Then(new MessageSucceeded());
        }
    }
}

using BlazorUI.Shared.Event.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Topic
{
    public class Chat : Totem.Timeline.Topic
    {
        public Chat()
        {

        }

        void When(SendMessage e)
        {
            Then(new MessageReceived(e.Message, e.User));
        }
        void When(MessageReceived e)
        {
            Then(new MessageSucceeded());
        }
    }
}

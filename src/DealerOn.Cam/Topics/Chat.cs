using DealerOn.Cam.Events.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace DealerOn.Cam.Topics
{
    public class Chat : Topic
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

using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace DealerOn.Cam.Events.Chat
{
    public class MessageReceived : Event
    {
        public readonly string Username;
        public readonly string Message;
        public MessageReceived(string message, string userName)
        {
            Message = message;
            Username = userName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Event.Chat
{
    public class MessageReceived : Totem.Timeline.Event
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

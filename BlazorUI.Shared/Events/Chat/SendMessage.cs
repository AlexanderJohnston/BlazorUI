using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Chat
{
    public class SendMessage : Command
    {
        public readonly string Message;
        public readonly string User;

        public SendMessage(string message, string user)
        {
            Message = message;
            User = user;
        }
    }
}

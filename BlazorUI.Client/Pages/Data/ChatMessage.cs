using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Data
{
    public class ChatMessage
    {
        public string User;
        public string Message;

        public ChatMessage() { }

        public ChatMessage(string user, string message)
        {
            User = user;
            Message = message;
        }

    }
}

using BlazorUI.Shared.Events.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries
{
    public class ChatQuery : Query
    {
        public int Count;
        public List<string> Messages = new List<string>();

        void Given(MessageReceived e)
        {
            Messages.Add(e.Username + ": " + e.Message);
            Count++;
        }
    }
}

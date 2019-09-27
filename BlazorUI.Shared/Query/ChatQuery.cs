using BlazorUI.Shared.Event.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Query
{
    public class ChatQuery : Totem.Timeline.Query
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

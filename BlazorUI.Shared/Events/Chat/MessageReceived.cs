﻿using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Chat
{
    public class MessageReceived : Event
    {
        public readonly string Username;
        public readonly string Message;

        public MessageReceived(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
}
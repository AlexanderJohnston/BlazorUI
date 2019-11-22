using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Chat
{
    public class MessageAccepted : Event
    {
        public readonly Id LobbyId;
        public readonly string Message;
        public readonly Id UserId;

        public MessageAccepted(Id lobbyId, Id userId, string message)
        {
            LobbyId = lobbyId;
            UserId = userId;
            Message = message;
        }
    }
}

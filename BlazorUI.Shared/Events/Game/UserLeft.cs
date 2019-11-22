using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class UserLeft : Event
    {
        public readonly Id LobbyId;
        public readonly Id UserId;

        public UserLeft(Id lobbyId, Id userId)
        {
            LobbyId = lobbyId;
            UserId = userId;
        }
    }
}

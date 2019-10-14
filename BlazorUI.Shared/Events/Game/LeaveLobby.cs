using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class LeaveLobby : Command
    {
        public readonly Id UserId;
        public readonly Id LobbyId;

        public LeaveLobby(Id userId, Id lobbyId)
        {
            UserId = userId;
            LobbyId = lobbyId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class JoinLobby : Command
    {
        public readonly Id UserId;
        public readonly Id LobbyId;

        public JoinLobby(Id userId, Id lobbyId)
        {
            UserId = userId;
            LobbyId = lobbyId;
        }
    }
}

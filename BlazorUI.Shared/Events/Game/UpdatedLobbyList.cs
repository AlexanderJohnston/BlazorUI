using System;
using System.Collections.Generic;
using System.Linq;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class UpdatedLobbyList : Event
    {
        public readonly IEnumerable<Lobby> Lobbies;
        public readonly IEnumerable<Id> LobbyIds;

        public UpdatedLobbyList(IEnumerable<Lobby> lobbies)
        {
            Lobbies = lobbies;
            LobbyIds = lobbies.Select(room => room.LobbyId);
        }
    }
}
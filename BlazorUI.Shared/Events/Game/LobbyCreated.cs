using System.Collections.Generic;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class LobbyCreated : Event
    {
        public readonly Id LobbyId;
        public readonly IEnumerable<Id> UserIds;

        public LobbyCreated(Id lobby, IEnumerable<Id> users)
        {
            LobbyId = lobby;
            UserIds = users;
        }
    }
}
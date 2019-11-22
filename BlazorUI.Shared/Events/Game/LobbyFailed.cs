using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class LobbyFailed : Event
    {
        public readonly Id LobbyId;
        public readonly string Reason;

        public LobbyFailed(Id lobbyId, string reason)
        {
            LobbyId = lobbyId;
            Reason = reason;
        }
    }
}

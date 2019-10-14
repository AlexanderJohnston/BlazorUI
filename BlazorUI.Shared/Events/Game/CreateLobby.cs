using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Game
{
    public class CreateLobby : Command
    {
        public readonly string AdminName;
        public readonly string LobbyName;

        public CreateLobby(string lobby, string admin)
        {
            LobbyName = lobby;
            AdminName = admin;
        }
    }
}

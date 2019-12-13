using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totem;

namespace BlazorUI.Shared.Events.Game
{
    public class Lobby
    {
        public Id LobbyId { get; set; }
        public List<Id> Users { get; set; }
        public List<Id> Admins { get; set; }
        public Lobby(Id Id, IEnumerable<Id> admins)
        {
            LobbyId = Id;
            Admins = admins.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Data
{
    public class ClientLobby
    {
        public string User;
        public string LobbyId;

        public ClientLobby(string lobby, string user)
        {
            LobbyId = lobby;
            User = user;
        }
    }
}

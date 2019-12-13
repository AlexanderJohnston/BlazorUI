using BlazorUI.Shared.Events.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries.Game
{
    public class LobbyList : Query
    {
        public List<Lobby> Lobbies = new List<Lobby>();

        void Given(UpdatedLobbyList e)
        {
            Lobbies.AddRange(e.Lobbies);
        }
        void Given(LeaveLobby e) 
        {
            if (LobbyExists(e.LobbyId) && UserInLobby(e.LobbyId, e.UserId))
            {
                SelectLobby(e.LobbyId).Users.Remove(e.UserId);
            }
        }
        void Given(JoinLobby e) 
        {
            if (LobbyExists(e.LobbyId))
            {
                SelectLobby(e.LobbyId).Users.Add(e.UserId);
            }
        }

        private Lobby SelectLobby(Id lobbyId) => Lobbies.First(lobby => lobby.LobbyId == lobbyId);
        private bool LobbyExists(Id lobbyId) => Lobbies.Any(lobby => lobby.LobbyId == lobbyId);
        private bool UserInLobby(Id lobbyId, Id userId) => SelectLobby(lobbyId).Users.Any(user => user == userId);
    }
}

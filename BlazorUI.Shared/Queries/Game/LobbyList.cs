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
        public Dictionary<Id, List<Id>> Lobbies = new Dictionary<Id, List<Id>>();

        void Given(LobbyCreated e)
        {
            var admins = e.UserIds.ToList();
            var lobby = e.LobbyId;
            Lobbies.Add(lobby, admins);
        }
        void Given(LeaveLobby e) 
        {
            if (LobbyExists(e.LobbyId) && UserInLobby(e.LobbyId, e.UserId))
            {
                SelectLobby(e.LobbyId).Value.Remove(e.UserId);
            }
        }
        void Given(JoinLobby e) 
        {
            if (LobbyExists(e.LobbyId))
            {
                SelectLobby(e.LobbyId).Value.Add(e.UserId);
            }
        }

        private KeyValuePair<Id, List<Id>> SelectLobby(Id lobbyId) => Lobbies.First(lobby => lobby.Key == lobbyId);
        private bool LobbyExists(Id lobbyId) => Lobbies.Any(lobby => lobby.Key == lobbyId);
        private bool UserInLobby(Id lobbyId, Id userId) => SelectLobby(lobbyId).Value.Any(user => user == userId);
    }
}

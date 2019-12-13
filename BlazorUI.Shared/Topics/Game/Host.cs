using BlazorUI.Shared.Events.Chat;
using BlazorUI.Shared.Events.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics.Game
{
    public class Host : Topic
    {
        public List<Lobby> Lobbies = new List<Lobby>();

        void When(JoinLobby e)
        {
            if (LobbyExists(e.LobbyId))
            {
                SelectLobby(e.LobbyId).Users.Add(e.UserId);
                Then(new UserJoined(e.LobbyId, e.UserId));
            }
        }

        void When(LeaveLobby e)
        {
            if (LobbyExists(e.LobbyId) && UserInLobby(e.LobbyId, e.UserId))
            {
                SelectLobby(e.LobbyId).Users.Remove(e.UserId);
                Then(new UserLeft(e.LobbyId, e.UserId));
            }
        }

        void When(CreateLobby e)
        {
            var lobby = Id.From(e.LobbyName);
            if (LobbyExists(lobby))
            {
                Then(new LobbyFailed(lobby, "Already exists in the lobby list."));
                return;
            }
            var admins = new List<Id>() { Id.From(e.AdminName) };
            Lobbies.Add(new Lobby(lobby, admins));
            Then(new UpdatedLobbyList(Lobbies));
        }

        void When(MessageReceived e)
        {
            var userId = Id.From(e.Username);
            if (LobbyExists(e.LobbyId) && UserInLobby(e.LobbyId, userId))
            {
                Then(new MessageAccepted(e.LobbyId, userId, e.Message));
            }
        }

        private Lobby SelectLobby(Id lobbyId) => Lobbies.First(lobby => lobby.LobbyId == lobbyId);
        private bool LobbyExists(Id lobbyId) => Lobbies.Any(lobby => lobby.LobbyId == lobbyId);
        private bool UserInLobby(Id lobbyId, Id userId) => SelectLobby(lobbyId).Users.Any(user => user == userId);
    }
}

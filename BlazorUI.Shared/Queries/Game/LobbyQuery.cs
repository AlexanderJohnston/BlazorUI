using BlazorUI.Shared.Events.Chat;
using BlazorUI.Shared.Events.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Queries.Game
{
    public class LobbyQuery : Query
    {
        static Many<Id> RouteFirst(UpdatedLobbyList e) => Many.Of(e.LobbyIds);
        static Id Route(UserJoined e) => e.LobbyId;
        static Id Route(UserLeft e) => e.LobbyId;
        static Id Route(MessageAccepted e) => e.LobbyId;

        public Id Lobby;
        public List<Id> Users = new List<Id>();
        public List<string> Messages = new List<string>();

        void Given(MessageAccepted e)
        {
            var sb = new StringBuilder();
            sb.Append(e.UserId);
            sb.Append(": ");
            sb.Append(e.Message);
            Messages.Add(sb.ToString());
        }

        void Given(UpdatedLobbyList e)
        {
            Lobby = Id;
            var lobby = e.Lobbies.Where(room => room.LobbyId == Id).First();
            Users = lobby.Users.ToList();
        }

        void Given(UserJoined e)
        {
            Users.Add(e.UserId);
        }

        void Given(UserLeft e)
        {
            if (Users.Any(user => user == e.UserId))
            {
                Users.Remove(e.UserId);
            }
        }
    }
}

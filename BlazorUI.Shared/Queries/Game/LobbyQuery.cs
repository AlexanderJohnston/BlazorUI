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
        static Id RouteFirst(LobbyCreated e) => e.LobbyId;
        static Id Route(UserJoined e) => e.LobbyId;
        static Id Route(UserLeft e) => e.LobbyId;
        static Id Route(MessageAccepted e) => e.LobbyId;

        public Id LobbyId;
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

        void Given(LobbyCreated e)
        {
            LobbyId = e.LobbyId;
            if (e.UserIds.Any())
            {
                Users = e.UserIds.ToList();
            }
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

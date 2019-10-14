using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Events.Chat
{
    public class MessageReceived : Event
    {
        public string Username;
        public string Message;
        public Id LobbyId;

        public MessageReceived(string username, string message, Id lobby)
        {
            Username = username;
            Message = message;
            LobbyId = lobby;
        }
    }
}

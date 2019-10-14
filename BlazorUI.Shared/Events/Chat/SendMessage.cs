using Totem.Timeline;

namespace BlazorUI.Shared.Events.Chat
{
    public class SendMessage : Command
    {
        public readonly string Message;
        public readonly string User;
        public readonly string Lobby;

        public SendMessage(string message, string user, string lobby)
        {
            Message = message;
            User = user;
            Lobby = lobby;
        }
    }
}

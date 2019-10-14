namespace BlazorUI.Client.Pages.Data
{
    public class ChatMessage
    {
        public string User;
        public string Message;
        public string Lobby;

        public ChatMessage(string lobby, string user, string message)
        {
            Lobby = lobby;
            User = user;
            Message = message;
        }

    }
}

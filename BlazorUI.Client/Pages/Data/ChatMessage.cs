namespace BlazorUI.Client.Pages.Data
{
    public class ChatMessage
    {
        public string User;
        public string Message;

        public ChatMessage(string user, string message)
        {
            User = user;
            Message = message;
        }

    }
}

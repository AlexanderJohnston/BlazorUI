using BlazorUI.Shared.Events.Chat;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    //[Profile]
    public class Chat : Topic
    {
        void When(SendMessage e)
        {
            var lobby = Id.From(e.Lobby);
            Then(new MessageReceived(e.Message, e.User, lobby));
        }

        void When(MessageReceived e)
        {
            Then(new MessageSucceeded());
        }
    }
}


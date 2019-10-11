using BlazorUI.Shared.Events.Chat;
using BlazorUI.Shared.Services.Aspect;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    [Profile]
    public class Chat : Topic
    {
        void When(SendMessage e)
        {
            Then(new MessageReceived(e.Message, e.User));
        }

        void When(MessageReceived e)
        {
            Then(new MessageSucceeded());
        }
    }
}

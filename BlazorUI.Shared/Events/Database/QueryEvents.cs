using Totem.Timeline;

namespace BlazorUI.Shared.Events.Database
{
    public class QueryEvents : Command
    {
        public readonly int Count;

        public QueryEvents(int count)
        {
            Count = count;
        }
    }
}

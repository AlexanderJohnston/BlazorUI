using Totem.Runtime;

namespace BlazorUI.Server
{
    public class LogMonitorService : Notion
    {
        public void Test()
        {
            var log = Notion.Traits.Log.ResolveDefaultTyped(this);
        }
    }
}

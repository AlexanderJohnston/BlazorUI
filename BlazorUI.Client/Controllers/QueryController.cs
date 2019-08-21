using Blazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class QueryController
    {
        private HubConnectionBuilder _builder { get; set; }
        private HubConnection _connection { get; set; }
        public QueryController(HubConnectionBuilder hub)
        {
            _builder = hub;
            Connect("/hubs/query");
        }
        private void Connect (string hubUrl)
        {
            _connection = 
                _builder.WithUrl(hubUrl,
                    options =>
                    {
                        options.LogLevel = SignalRLogLevel.Debug;
                        options.Transport = HttpTransportType.WebSockets;
                        options.SkipNegotiation = true;
                    })
                .Build();
        }
        public void SubscribeToQuery(string etag, Func<string, Task> handler)
        {
            _connection.On<string>(etag, async (message) => { await handler(message); });
        }
    }
}

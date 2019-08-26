using Blazor.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class QueryController
    {
        private Dictionary<string, Func<string,Task>> _etagSubscriptions { get; set; }
        private HubConnectionBuilder _builder { get; set; }
        private HubConnection _connection { get; set; }
        public QueryController(HubConnectionBuilder hub)
        {
            _builder = hub;
            Connect("/hubs/query");
            _etagSubscriptions = new Dictionary<string, Func<string, Task>>();
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
            _connection.On <string> ("onChanged", OnChanged);
        }
        public void SubscribeToQuery(string etag, Func<string, Task> handler)
        {
            _connection.InvokeAsync("SubscribeToChanged", etag);
            _etagSubscriptions.Add(etag, handler);
        }
        public Task OnChanged(string etag)
        {
            Debug.WriteLine(etag);
            return _etagSubscriptions.First(sub => sub.Key == etag).Value.Invoke(etag);
        }
        public string Test()
        {
            return _connection.ToString();
        }
    }
}

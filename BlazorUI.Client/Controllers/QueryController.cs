using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class QueryController : ComponentBase
    {
        public Dictionary<string, Func<string,Task>> _etagSubscriptions { get; set; }
        public HubConnectionBuilder _builder { get; set; }
        public HubConnection _connection { get; set; }
        public QueryController(HubConnectionBuilder hub)
        {
            _builder = hub;
            Connect("/hubs/query");
            _etagSubscriptions = new Dictionary<string, Func<string, Task>>();
        }
        public void Connect (string hubUrl)
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
            _connection.OnClose(close =>
            {
                Debug.WriteLine("The SignalR connection was closed! " + close.ToString());
                return Task.CompletedTask;
            });
            _connection.StartAsync();
        }
        public void SubscribeToQuery(string etag, Func<string, Task> handler)
        {
            Console.WriteLine("Entered the SubscribeToQuery method of QueryController.");
            _connection.InvokeAsync("SubscribeToChanged", etag);
            _etagSubscriptions.Add(etag, handler);
        }
        public Task OnChanged(object etag)
        {
            Debug.WriteLine(etag);
            Debug.WriteLine("Totem said hello!");
            var checkpointIndex = etag.ToString().IndexOf("@");
            var subscription = etag.ToString().Substring(1, checkpointIndex - 1);
            Debug.WriteLine(subscription);

            return _etagSubscriptions.First(sub => sub.Key == subscription).Value.Invoke(subscription.ToString());
        }
        public string Test()
        {
            return _connection.ToString();
        }
    }
}

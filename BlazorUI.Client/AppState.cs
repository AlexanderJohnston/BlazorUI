using BlazorUI.Client.Queries;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Client
{
    public class AppState : ComponentBase
    {
        private HttpClient _http { get; set; }
        
        /// <summary>
        ///     https://docs.microsoft.com/en-us/aspnet/core/blazor/dependency-injection?view=aspnetcore-3.0#use-di-in-services
        ///     Apparently this is de wae.
        /// </summary>
        /// <param name="query"></param>
        public AppState(QueryController query, HttpClient http)
        {
            _query = query;
            _http = http;
        }

        /// <summary>
        /// Map of queries and their routes injected by the server.
        /// </summary>
        private List<TimelineRoute> _queryMap { get; set; }

        /// <summary>
        /// Stores the list of Query types and the list of callback functions which are interested in changes to that query.
        /// </summary>
        private Dictionary<Type, List<Func<object, Task>>> _viewSubscriptions { get; set; } = new Dictionary<Type, List<Func<object, Task>>>();

        /// <summary>
        /// Subscribes to changes on the server using SignalR.
        /// </summary>
        public QueryController _query { get; set; }

        /// <summary>
        ///     Subscribes a callback handler method to a <see cref="Query"/> if there exists known route to it.
        /// </summary>
        /// <typeparam name="T">A type inherting from <see cref="Query"/></typeparam>
        /// <param name="handler">Handler method which accepts a JSON representation of a <see cref="Query"/>.</param>
        /// <returns></returns>
        public async Task Subscribe<T>(Func<object, Task> handler = null)
        {
            if (_queryMap == null)
            {
                var request = await _http.GetAsync("/querymap/get/");
                var response = await request.Content.ReadAsStringAsync();
                Console.WriteLine("List of queries and their routes: " + response);
                _queryMap = JsonConvert.DeserializeObject<List<TimelineRoute>>(response);
            }
            var type = typeof(T);
            Debug.WriteLine("Subscribing to a query: " + type.Name);
            if (_queryMap.Any(map => map.QueryType == type))
            {
                Console.WriteLine("Matching route found.");
                var timeline = _queryMap.First(map => map.QueryType == type);
                var etag = SanitizeETag(await ReadETag(timeline.Route));
                _query.SubscribeToQuery(etag, timeline.Route, ReadSubscription<T>);
                if (handler != null && _viewSubscriptions.Any(view => view.Key == typeof(T)))
                {
                    var queryView = _viewSubscriptions.First(view => view.Key == typeof(T));
                    queryView.Value.Add(handler);
                }
                else if (handler != null)
                {
                    _viewSubscriptions.Add(type, new List<Func<object, Task>>() { handler });
                    // If we don't initialize this handler then it will be null when the subscribing component is rendered.
                    await ReadSubscription<T>("Initialization of " + typeof(T), timeline.Route);
                }
            }
        }

        public async Task<T> ReadSubscription<T>(string message, string route)
        {
            Console.WriteLine("Message from SignalR: " + message);
            var queryRequest = await _http.GetAsync(route);
            if (queryRequest.IsSuccessStatusCode)
            {
                var response = await queryRequest.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + response);
                var query = JsonConvert.DeserializeObject<T>(response);
                Console.WriteLine("Deserialized response into type ." + typeof(T));
                var ETag = queryRequest.Headers.ETag.Tag.ToString() != null
                    ? queryRequest.Headers.ETag.Tag
                    : ("null etag on message: " + message);
                foreach (var callback in _viewSubscriptions.First(view => view.Key == typeof(T)).Value)
                {
                    await callback.Invoke(query);
                }
                return (query);
            }
            else
            {
                Console.WriteLine("Failed to retrieve the update from SignalR on: " + route);
                return default(T);
            }
        }

        private async Task<string> ReadETag(string route)
        {
            Debug.WriteLine("Looking up the ETag for: " + route);
            var request = await _http.GetAsync(route);
            if (request.IsSuccessStatusCode)
            {
                Debug.WriteLine(string.Format("Status: {0} on route {1}", request.StatusCode, route));
                return request.Headers.ETag.Tag.ToString();
            }
            return string.Empty;
        }

        public string SanitizeETag(string etag)
        {
            string subscription = etag.Trim(new char[] { '"' });
            var checkpoint = subscription.IndexOf("@");
            if (checkpoint > 0)
            {
                var span = subscription.AsSpan();
                var builder = new StringBuilder();
                for (int i = 0; i < span.Length; i++)
                {
                    if (i < checkpoint)
                        builder.Append(span[i]);
                }
                return builder.ToString();
            }
            else 
            {
                return subscription;
            }
        }
    }
}

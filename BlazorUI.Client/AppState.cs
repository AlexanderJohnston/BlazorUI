using BlazorUI.Client.Pages.Data;
using BlazorUI.Client.Queries;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Client
{
    /// <summary>
    ///  What it does:
    ///  * Creates a new SignalR subscription to changes made in a specific Totem Query.
    ///  
    ///  How does it know where the queries live?
    ///  * The server injects a list of known routes and their query type into the client before sending the client to the user.
    ///  
    ///  How does it bind them?
    ///  * A Razor Page will provide a handler or callback function responsible for storing the new object when changes are made.
    ///  * AppState is only responsible for receiving the changes from the server and invoking the callback methods of all interested observers (Pages).
    ///  
    ///  How does it know if something truly changed?
    ///  * Whenever a Query is mutated on the server it will increase its checkpoint or version number by 1.
    ///  * The server sends an etag in the form of "NameQuery/instance@checkpoint" or "StatusQuery/Alex@3".
    ///  * AppState then looks to see if it has a TimelineRoute which maps that etag back to a route on the server where it can fetch the data at its leisure.
    /// </summary>
    public class AppState : ComponentBase
    {
        private class UICallBack
        {
            public UICallBack(MethodInfo handler, object instance, Type type)
            {
                Handler = handler;
                Instance = instance;
                InstanceType = type;
            }
            public Type InstanceType;
            public MethodInfo Handler;
            public object Instance;
        }
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
        /// Stores Query types and the list of callback functions which are observing changes to that query.
        /// </summary>
        private Dictionary<Type, List<UICallBack>> _viewSubscriptions { get; set; } = new Dictionary<Type, List<UICallBack>>();

        /// <summary>
        /// Subscribes to changes on the server using SignalR.
        /// </summary>
        public QueryController _query { get; set; }

        /// <summary>
        ///     Subscribes a callback handler method to a <see cref="Query"/> if there exists known route to it.
        /// </summary>
        /// <typeparam name="T">A type inherting from <see cref="Query"/></typeparam>
        /// <param name="handler">Handler method which accepts a JSON representation of a <see cref="Query"/>.</param>
        /// <param name="subscriptionId">Optional: Subscribe to an instance of a query if you know the subscription ID (etag) for it.</param>
        /// <returns></returns>
        public async Task Subscribe<T>(MethodInfo handler = null, object instance = null, string subscriptionId = null, Type instanceType = null)
        {
            Debug.WriteLine("Starting subscription method in App State.");
            var type = typeof(T);
            Debug.WriteLine($"Type[{type}]: Subscription Id: {subscriptionId}");
            if (_queryMap == null)
            {
                _queryMap = await ReadQueryMap();
            }
            Debug.WriteLine("Subscribing to a query: " + type.Name);
            if (QueryExists(type))
            {
                Console.WriteLine("Matching route found.");
                var timeline = SelectQuery(type);
                var etag = SanitizeETag(await ReadETag(timeline.Route, subscriptionId));
                _query.SubscribeToQuery(etag, timeline.Route, ReadSubscription<T>);
                if (handler != null && _viewSubscriptions.Any(view => view.Key == typeof(T)))
                {
                    var queryView = _viewSubscriptions.First(view => view.Key == typeof(T));
                    queryView.Value.Add(new UICallBack(handler, instance, instanceType));
                }
                else if (handler != null)
                {
                    _viewSubscriptions.Add(type, new List<UICallBack> { new UICallBack(handler, instance, instanceType) });
                    // If we don't initialize this handler then it will be null when the subscribing component is rendered.
                    //await ReadSubscription<T>("Initialize " + typeof(T), timeline.Route);
                }
            }
        }
        private Func<object, Task> ConvertMethodInfo<T>(UICallBack callback)
        {
            var callerType = callback.Instance.GetType();
            var instanceParam = Expression.Parameter(callerType, "instance");
            var memberParam = Expression.Parameter(typeof(T), "query");
            return Expression.Lambda<Func<object, Task>>(
                Expression.Call(instanceParam, callback.Handler, memberParam)).Compile();
        }

        private TimelineRoute SelectQuery(Type type) => _queryMap.First(map => map.QueryType == type);
        private bool QueryExists(Type type) => _queryMap.Any(map => map.QueryType == type);

        /// <summary>
        ///     Downloads a map from the server which matches a Query type to a controller route.
        /// </summary>
        /// <returns></returns>
        private async Task<List<TimelineRoute>> ReadQueryMap()
        {
            var request = await _http.GetAsync("/querymap/get/");
            var response = await request.Content.ReadAsStringAsync();
            Debug.WriteLine("List of queries and their routes: " + response);
            return JsonConvert.DeserializeObject<List<TimelineRoute>>(response);
        }

        public async Task<T> ReadSubscription<T>(string message, string route)
        {
            Debug.WriteLine("Message from SignalR: " + message);
            var queryRequest = await _http.GetAsync(route);
            if (queryRequest.IsSuccessStatusCode)
            {
                var response = await queryRequest.Content.ReadAsStringAsync();
                Debug.WriteLine("Response: " + response);
                var query = JsonConvert.DeserializeObject<T>(response);
                Debug.WriteLine("Deserialized response into type: " + typeof(T));
                var ETag = queryRequest.Headers.ETag.Tag.ToString() != null
                    ? queryRequest.Headers.ETag.Tag
                    : ("null etag on message: " + message);
                var queryParameter = Expression.Parameter(typeof(T), "query");
                var readQueryParameter = Expression.Convert(queryParameter, typeof(object));
                foreach (var callback in _viewSubscriptions.First(view => view.Key == typeof(T)).Value)
                    await ConvertMethodInfo<T>(callback).Invoke(query);
                return (query);
            }
            else
            {
                Debug.WriteLine("Failed to " + message + " the update from SignalR on: " + route + " with status: " + queryRequest.StatusCode);
                return default;
            }
        }

        private async Task<string> ReadETag(string route, string subscriptionId = null)
        {
            HttpResponseMessage request;
            if (subscriptionId != null)
            {
                Debug.WriteLine("Looking up the ETag for: " + route + ", checkpoint: " + subscriptionId);
                var query = new QueryId(subscriptionId);
                var content = JsonConvert.SerializeObject(query);
                request = await _http.PostAsJsonAsync(route, content);
            }
            else
            {
                Debug.WriteLine("Looking up the ETag for: " + route);
                request = await _http.GetAsync(route);
            }
            if (request.IsSuccessStatusCode)
            {
                Debug.WriteLine(string.Format("Status: {0} on route {1}", request.StatusCode, route));
                return request.Headers.ETag.Tag.ToString();
            }
            else
            {
                Debug.WriteLine(string.Format("Status: {0} on route {1}", request.StatusCode, route));
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

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

        private Func<object, Task> QueryCallback;

        public QueryController _query { get; set; }

        private Dictionary<Type, View> _views { get; set; } = new Dictionary<Type, View>();

        public void Register(Type type)
        {

        }
        public void Subscribe<T>(string etag, string route, Func<object, Task> handler = null)
        {
            Debug.WriteLine("Sanitizing etag for subscription: " + etag);
            var sanitizedTag = SanitizeETag(etag);
            if (handler != null)
            {
                QueryCallback = handler;
            }
            _query.SubscribeToQuery(sanitizedTag, route, ReadSubscription<T>);
            NotifyStateChanged();
        }

        public event Action OnChange;

        public List<string> Etags { get; } = new List<string>();

        public void AddEtag(string etag)
        {
            Etags.Add(etag);
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
        public async Task<T> ReadSubscription<T>(string message, string route)
        {
            Console.WriteLine("Message from SignalR: " + message);
            Console.WriteLine("Made it into ReadEcho on the Razor Page.");
            var queryRequest = await _http.GetAsync(route);
            if (queryRequest.IsSuccessStatusCode)
            {
                Console.WriteLine("Successful retrieved the new " + message);
                var response = await queryRequest.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + response);
                var query = JsonConvert.DeserializeObject<T>(response);
                Console.WriteLine("Deserialized response into type ." + typeof(T));
                var ETag = queryRequest.Headers.ETag.Tag.ToString() != null 
                    ? queryRequest.Headers.ETag.Tag 
                    : ("null etag on message: " + message);
                Console.WriteLine("Success Status in ReadEcho on Razor Page");
                await QueryCallback(query);
                return (query);
            }
            else
            {
                Console.WriteLine("Fail Status in ReadEcho on Razor Page");
                return default(T);
            }
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

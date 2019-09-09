using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class AppState
    {
        [Inject] public QueryController _query { get; set; }

        private Dictionary<Type, View> _views { get; set; } = new Dictionary<Type, View>();

        public void Register(Type type)
        {

        }
        public void Subscribe(string etag, Func<string, Task> handler)
        {
            if (etag.Contains("\""))
            {
                etag = etag.Substring(1, etag.Length - 2);
            }
            Debug.WriteLine("Subscribed to " + etag);
            _query.SubscribeToQuery(etag, handler);
        }

        public event Action OnChange;

        public List<string> Etags { get; } = new List<string>();

        public void AddEtag(string etag)
        {
            Etags.Add(etag);
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

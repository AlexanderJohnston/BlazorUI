using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class AppState
    {
        public event Action OnChange;

        public List<string> Etags { get; }
        public void AddEtag(string etag)
        {
            Etags.Add(etag);
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

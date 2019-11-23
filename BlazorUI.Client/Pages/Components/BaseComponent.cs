using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using System;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class BaseComponent<TQuery> : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public TQuery Query;
        public async Task ReadQuery(object query)
        {
            this.Query = (TQuery)query;
            StateHasChanged();
        }
    }
}
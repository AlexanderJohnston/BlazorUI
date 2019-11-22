using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class EchoComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public EchoQuery Echo { get; set; }
        public string EchoEtag = "No Echo Etag";

        protected async override Task OnInitializedAsync()
        {
            await _appState.Subscribe<EchoQuery>(ReadQuery<EchoQuery>);
            StateHasChanged();
        }

        public async Task ReadQuery<T>(object query)
        {
            this.Echo = (EchoQuery)query;
            StateHasChanged();
        }

        public async Task SendEcho()
        {
            var echoResponse = await _http.PostAsync("/status/sendecho", null);
            Console.WriteLine(echoResponse.StatusCode.ToString() + " is the echo status code.");
        }
    }
}

using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class EchoComponent : BaseComponent<EchoComponent>
    {
        public string EchoEtag = "No Echo Etag";
        public EchoQuery Echo { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
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

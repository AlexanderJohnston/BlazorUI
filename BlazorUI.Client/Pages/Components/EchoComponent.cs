using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class EchoComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public EchoQuery Echo { get; set; } = new EchoQuery();
        public string EchoEtag = "No Echo Etag";

        protected async override Task OnInitializedAsync()
        {
            var echoResponse = await _http.GetAsync("/status/getecho");
            var EchoEtag = echoResponse.Headers.ETag.Tag;
            var echoCheckpoint = EchoEtag.IndexOf("@");
            string echoSubscription;
            if (echoCheckpoint > 0)
            {
                echoSubscription = EchoEtag.Substring(1, echoCheckpoint - 1);
            }
            else
            {
                echoSubscription = EchoEtag;
            }
            //EchoEtag = echoEtag + " => " + echoSubscription;
            _appState.Subscribe(echoSubscription, ReadEcho);
            StateHasChanged();
        }

        public async Task ReadEcho(string message)
        {
            Console.WriteLine("Made it into ReadEcho on the Razor Page.");
            var echoRequest = await _http.GetAsync("/status/getecho");
            if (echoRequest.IsSuccessStatusCode)
            {
                Echo = JsonConvert.DeserializeObject<EchoQuery>(await echoRequest.Content.ReadAsStringAsync());
                EchoEtag = echoRequest.Headers.ETag.Tag;
                Console.WriteLine("Success Status in ReadEcho on Razor Page");
            }
            else
            {
                Console.WriteLine("Fail Status in ReadEcho on Razor Page");
            }
            StateHasChanged();
        }

        public async Task SendEcho()
        {
            var echoResponse = await _http.PostAsync("/status/sendecho", null);
            Console.WriteLine(echoResponse.StatusCode.ToString() + " is the echo status code.");
        }
    }
}

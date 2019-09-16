﻿using DealerOn.Cam.Queries;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            await ReadEcho("On Initialization");
            Console.WriteLine("EchoEtag = " + EchoEtag);
            _appState.Subscribe<EchoQuery>(EchoEtag, "/status/getecho", ReadQuery<EchoQuery>);
            StateHasChanged();
        }

        

        public async Task ReadQuery<T>(object query)
        {
            var queryResponse = (EchoQuery)query;
            this.Echo = queryResponse;
            StateHasChanged();
        }

        public async Task ReadEcho(string message)
        {
            Console.WriteLine("Message from SignalR: " + message);
            Console.WriteLine("Made it into ReadEcho on the Razor Page.");
            var echoRequest = await _http.GetAsync("/status/getecho");
            if (echoRequest.IsSuccessStatusCode)
            {
                Console.WriteLine("Successful retrieved the new " + message);
                var response = await echoRequest.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + response);
                Echo = JsonConvert.DeserializeObject<EchoQuery>(await echoRequest.Content.ReadAsStringAsync());
                Console.WriteLine("Deserialized response into type ." + typeof(Echo));
                EchoEtag = echoRequest.Headers.ETag.Tag.ToString() != null ? echoRequest.Headers.ETag.Tag : ("null etag on message: " + message);
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
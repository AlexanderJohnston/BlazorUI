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
    public class ImportComponent : ComponentBase
    {
        public string Echo = "No Echo";
        public string EchoEtag = "No Echo Etag";
        public string StatusEtag = "0";
        public string RegionsEtag = "0";
        public string Status = "X";
        private int _checkpoint = 0;
        protected string _headingFontStyle = "italic";
        protected string _headingText = "Put on your new Blazor!";
        public string imports = "Not Imported";
        public HttpResponseMessage _importResponse;
        public string Response = "";
        public string manifestData = "Not Found";
        public string manifestStatus = "Not Started";
        public string TestString = "-";

        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }

        protected async override Task OnInitializedAsync()
        {
            //var statusResponse = await Http.GetAsync("/status/getstatus");
            //statusEtag = statusResponse.Headers.ETag.Tag;
            var echoResponse = await _http.GetAsync("/status/getecho");
            var echoEtag = echoResponse.Headers.ETag.Tag;
            var echoCheckpoint = echoEtag.ToString().IndexOf("@");
            string echoSubscription;
            if (echoCheckpoint > 0)
            {
                echoSubscription = echoEtag.Substring(1, echoCheckpoint - 1);
            }
            else
            {
                echoSubscription = echoEtag;
            }
            EchoEtag = echoEtag + " => " + echoSubscription;
            _appState.Subscribe<EchoQuery>(echoSubscription, "/status/getecho");

            var regionsResponse = await _http.GetAsync("/regions/getstatus");
            var regionsEtag = regionsResponse.Headers.ETag.Tag;
            var regionsCheckpoint = regionsEtag.ToString().IndexOf("@");
            string regionsSubscription;
            if (regionsCheckpoint > 0)
            {
                regionsSubscription = regionsEtag.Substring(1, regionsCheckpoint - 1);
            }
            else
            {
                regionsSubscription = regionsEtag;
            }
            RegionsEtag = regionsEtag + " => " + regionsSubscription;
            _appState.Subscribe<RegionsQuery>(regionsSubscription, "/regions/getstatus");
            _appState.OnChange += StateHasChanged;
        }
        public async Task ReadEcho(string message)
        {
            Console.WriteLine("Made it into ReadEcho on the Razor Page.");
            var echoRequest = await _http.GetAsync("/status/getecho");
            if (echoRequest.IsSuccessStatusCode)
            {
                Echo = JsonPrettify(await echoRequest.Content.ReadAsStringAsync());
                Console.WriteLine("Success Status in ReadEcho on Razor Page");
            }
            else
            {
                Console.WriteLine("Fail Status in ReadEcho on Razor Page");
                Echo = echoRequest.StatusCode.ToString();
            }
        }
        public async Task ReadRegions(string message)
        {
            _checkpoint++;
            manifestStatus = "SignalR Received Regions. Checkpoint: " + _checkpoint;
            Console.WriteLine("Made it into ReadRegions on the Razor Page.");
            var regionsRequest = await _http.GetAsync("/regions/getstatus");
            if (regionsRequest.IsSuccessStatusCode)
            {
                imports = JsonPrettify(await regionsRequest.Content.ReadAsStringAsync());
                Console.WriteLine("Success Status in Regions on Razor Page");
            }
            else
            {
                Console.WriteLine("Fail Status in Regions on Razor Page");
                imports = regionsRequest.StatusCode.ToString();
            }
        }
        public async Task ReadStatus(string message)
        {
            _checkpoint++;
            manifestStatus = "SignalR Received Status. Checkpoint: " + _checkpoint;
            var statsuRequest = await _http.GetAsync("/regions/getstatus");
            if (statsuRequest.IsSuccessStatusCode)
            {
                Status = JsonPrettify(await statsuRequest.Content.ReadAsStringAsync());
            }
        }

        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        public async Task ReadImport(string response)
        {
            Response = response;
            manifestStatus = "SignalR Received.";
        }

        public async Task StartImport()
        {
            /*var connection = _hubBuilder.WithUrl("/hubs/query").Build();
            var regionsResponse = await Http.GetAsync("/api/regions");
            etag = regionsResponse.Headers.ETag.Tag;
            AppState.AddEtag(etag);*/

            //connection.On<string>(etag, async (message) => { await ReadImport(message); });
            //old
            //_importResponse = await Http.GetAsync("imports/test");
            _importResponse = await _http.PostAsync("imports/startimport", null);
            //imports = await _importResponse.Content.ReadAsStringAsync();
            imports = "Importing...";
        }

        public async Task StartManifest()
        {
            var manifestResponse = await _http.PostAsync("/regions/poststatus", null);
            //manifestStatus = manifestResponse.StatusCode.ToString();
        }
        public async Task SendEcho()
        {
            var echoResponse = await _http.PostAsync("/status/sendecho", null);
            Console.WriteLine(echoResponse.StatusCode.ToString() + " is the echo status code.");
        }
        public void Test()
        {
            TestString = "Clicked.";
        }

    }
}

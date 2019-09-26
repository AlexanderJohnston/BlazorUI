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
            
        }
        public async Task ReadQuery<T>(object query)
        {
            //var queryResponse = (EchoQuery)query;
            //this.Echo = queryResponse;
            StateHasChanged();
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
            _importResponse = await _http.PostAsync("imports/startimport", null);
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

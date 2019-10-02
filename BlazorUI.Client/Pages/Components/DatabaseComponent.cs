using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Shared.Queries;
using System.Net.Http;
using BlazorUI.Shared.Data;
using System.Text.Json;

namespace BlazorUI.Client.Pages.Components
{
    public class DatabaseComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public LegacyEventQuery Legacy { get; set; }
        public string DatabaseTag = "No Database Etag";

        protected async override Task OnInitializedAsync()
        {
            await _appState.Subscribe<LegacyEventQuery>(ReadQuery<LegacyEventQuery>);
            StateHasChanged();
        }

        public async Task ReadQuery<T>(object query)
        {

            var queryResponse = (LegacyEventQuery)query;
            this.Legacy = queryResponse;
            Console.WriteLine($"LegacyEventQuery Context: {queryResponse.Context}");
            Console.WriteLine($"LegacyEventQuery Created: {queryResponse.WhenCreated}");
            Console.WriteLine($"LegacyEventQuery Changed: {queryResponse.WhenChanged}");
            Console.WriteLine($"LegacyEventQuery Events: {queryResponse.Events.Count}");
            Console.WriteLine($"DatabaeComponent read query: {this.Legacy}{Environment.NewLine}" +
                $"Number of events: {this.Legacy?.Events?.Count}{Environment.NewLine}" +
                $"First event: {this.Legacy?.Events?.FirstOrDefault()}");
            StateHasChanged();
        }

        public async Task FetchEvents()
        {
            var echoResponse = await _http.PostAsync("/LegacyEvents/FetchEvents", null);
            Console.WriteLine(echoResponse.StatusCode.ToString() + " is the database legacy events status code.");
        }

        public List<LegacyEvent> SafeDeserialize()
        {
            Console.WriteLine("Safely deserializing the database events.");
            var whatever = new List<LegacyEvent>();
            foreach (var deserialized in Legacy.Events)
            {
                Console.WriteLine(deserialized.Position + " " + deserialized.Type + " "
                    + deserialized.Cause + " " + deserialized.Json);
                whatever.Add(deserialized);
            }
            return whatever;
        }
    }
}

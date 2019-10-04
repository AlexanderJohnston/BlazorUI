using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Shared.Queries;
using System.Net.Http;
using BlazorUI.Shared.Data;
using BlazorUI.Client.Pages.Data;

namespace BlazorUI.Client.Pages.Components
{
    public class DatabaseComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public LegacyEventQuery Legacy { get; set; }
        public BatchStatusQuery BatchStatus { get; set; }
        public string DatabaseTag = "No Database Etag";

        protected async override Task OnInitializedAsync()
        {
            await _appState.Subscribe<LegacyEventQuery>(ReadQuery<LegacyEventQuery>);
            await _appState.Subscribe<BatchStatusQuery>(ReadQuery<BatchStatusQuery>);
            StateHasChanged();
        }

        public async Task ReadQuery<T>(object query)
        {
            if (typeof(T) == typeof(BatchStatusQuery))
            {
                this.BatchStatus = (BatchStatusQuery)query;
                Console.WriteLine($"Batch download at {BatchStatus.PercentProgress}%");
            }
            else
            {
                this.Legacy = (LegacyEventQuery)query;
            }
            StateHasChanged();
        }

        public async Task FetchEvents()
        {
            var content = JsonConvert.SerializeObject(new BatchSize(1000));
            var echoResponse = await _http.PostAsJsonAsync("/LegacyEvents/FetchEvents", content);
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

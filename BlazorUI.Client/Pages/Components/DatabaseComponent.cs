using BlazorUI.Client.Pages.Data;
using BlazorUI.Shared.Data;
using BlazorUI.Shared.Queries;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    /// <summary>
    ///     This component is absolutely broken right now other than for initialization purposes.
    ///     It provides a viable path to display SQL data but the implementation is messy on the server side.
    /// </summary>
    public class DatabaseComponent : BaseComponent<DatabaseComponent>
    {
        public string DatabaseTag = "No Database Etag";
        public int NumberOfEvents;
        public BatchStatusQuery BatchStatus {get; set;}
        public LegacyEventQuery Legacy {get; set;}

        protected async override Task OnInitializedAsync()
        {
            //await _appState.Subscribe<LegacyEventQuery>(ReadQuery<LegacyEventQuery>);
            //await _appState.Subscribe<BatchStatusQuery>(ReadQuery<BatchStatusQuery>);
            //StateHasChanged();
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
            var content = JsonConvert.SerializeObject(new BatchSize(NumberOfEvents));
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

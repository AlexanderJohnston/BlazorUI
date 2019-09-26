using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerOn.Cam.Queries;
using System.Net.Http;
using BlazorUI.Client.Pages.Data;
using Newtonsoft.Json;
using System.Text;

namespace BlazorUI.Client.Pages.Components
{
    public class ChatComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public ChatQuery Chat { get; set; }
        public string ChatEtag = "No Chat Etag";
        public string CurrentMessage = "";
        public string CurrentUser = "Anon";

        protected async override Task OnInitializedAsync()
        {
            await _appState.Subscribe<ChatQuery>(ReadQuery<ChatQuery>);
            StateHasChanged();
        }

        public async Task ReadQuery<T>(object query)
        {
            var queryResponse = (ChatQuery)query;
            this.Chat = queryResponse;
            StateHasChanged();
        }

        public async Task SendMessage()
        {
            var chat = new ChatMessage(CurrentUser, CurrentMessage);
            var content = JsonConvert.SerializeObject(chat);
            var chatResponse = await _http.PostAsJsonAsync("/chat/send", content);
            Console.WriteLine(chatResponse.StatusCode.ToString() + " is the echo status code.");
        }
    }
}

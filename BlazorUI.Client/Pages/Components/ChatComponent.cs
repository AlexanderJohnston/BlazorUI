using BlazorUI.Client.Pages.Data;
using BlazorUI.Shared.Queries;
using BlazorUI.Shared.Queries.Game;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class ChatComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        public LobbyQuery Chat { get; set; }
        public LobbyList Lobbies { get; set; }
        public string ChatEtag = "No Chat Etag";
        public string CurrentMessage = "";
        public string CurrentUser = "Anon";
        public string CurrentLobby = "Random";
        public string CreateLobbyName = "Random";

        protected async override Task OnInitializedAsync()
        {
            Console.WriteLine(CurrentLobby);
            await _appState.Subscribe<LobbyQuery>(ReadQuery<LobbyQuery>, CurrentLobby);
            //await _appState.Subscribe<LobbyList>(ReadLobby<LobbyList>);
            StateHasChanged();
        }

        public async Task ReadQuery<T>(object query)
        {
            var queryResponse = (LobbyQuery)query;
            this.Chat = queryResponse;
            StateHasChanged();
        }

        public async Task ReadLobby<T>(object query)
        {
            var queryResponse = (LobbyList)query;
            this.Lobbies = queryResponse;
            StateHasChanged();
        }

        public async Task SendMessage()
        {
            var chat = new ChatMessage(CurrentLobby, CurrentUser, CurrentMessage);
            var content = JsonConvert.SerializeObject(chat);
            Console.WriteLine($"Message: {CurrentMessage}   User: {CurrentUser}{Environment.NewLine}" +
                $"Serialized: {content}");
            var chatResponse = await _http.PostAsJsonAsync("/chat/send", content);
            Console.WriteLine(chatResponse.StatusCode.ToString() + $" status code returned when sending a message to chat.");
        }

        public async Task JoinLobby()
        {
            await _appState.Subscribe<LobbyQuery>(ReadQuery<LobbyQuery>, CurrentLobby);
            StateHasChanged();
        }

        public async Task CreateLobby()
        {
            var lobby = new ClientLobby(CreateLobbyName, CurrentUser);
            var content = JsonConvert.SerializeObject(lobby);
            Console.WriteLine($"Create Lobby: {CreateLobbyName}   User: {CurrentUser}{Environment.NewLine}" +
                $"Serialized: {content}");
            var lobbyResponse = await _http.PostAsJsonAsync("/chat/create", content);
            Console.WriteLine(lobbyResponse.StatusCode.ToString() + $" status code returned when creating a new lobby.");
        }
    }
}

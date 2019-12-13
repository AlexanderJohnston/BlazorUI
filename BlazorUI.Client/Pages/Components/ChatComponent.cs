using BlazorUI.Client.Pages.Data;
using BlazorUI.Shared.Queries;
using BlazorUI.Shared.Queries.Game;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totem;

namespace BlazorUI.Client.Pages.Components
{
    public class ChatComponent : BaseComponent<ChatComponent>
    {
        public LobbyQuery Chat { get; set; } = new LobbyQuery().Bind(Id.From("General Chat"));
        public LobbyList Lobbies { get; set; }
        public string ChatEtag = "No Chat Etag";
        public string CurrentMessage = "";
        public string CurrentUser = "Anon";
        public string CurrentLobby = "Random";
        public string CreateLobbyName = "Random";

        //protected async override Task OnInitializedAsync()
        //{
        //    var finalizedCallback = new UICallBack(this, GetType(), typeof(LobbyQuery));
        //    await base.OnInitializedAsync();
        //    await _appState.Subscribe<LobbyQuery>(finalizedCallback, "General Chat");
        //}

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
            //await _appState.Subscribe<LobbyQuery>(ReadLobby<LobbyQuery>, CurrentLobby);
            //StateHasChanged();
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

using BlazorUI.Client.Pages.Data;
using BlazorUI.Server.Attributes;
using BlazorUI.Shared.Events.Chat;
using BlazorUI.Shared.Events.Game;
using BlazorUI.Shared.Queries;
using BlazorUI.Shared.Queries.Game;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Totem;
using Totem.Timeline.Mvc;

namespace BlazorUI.Server.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private IQueryServer _queries { get; set; }
        private ICommandServer _commands { get; set; }

        public ChatController(IQueryServer queries, ICommandServer commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet("[action]")]
        [TimelineQuery(typeof(ChatQuery))]
        public Task<IActionResult> Get() => _queries.Get<ChatQuery>();

        [HttpGet("[action]")]
        [TimelineQuery(typeof(LobbyList))]
        public Task<IActionResult> GetLobbies() => _queries.Get<LobbyList>();

        [HttpPost("[action]")]
        [TimelineQuery(typeof(LobbyQuery))]
        public Task<IActionResult> LobbySubscription([FromBody] string content)
        {
            var query = JsonConvert.DeserializeObject<QueryId>(content);
            return _queries.Get<LobbyQuery>(Id.From(query.Id));
        }

        [HttpPost("[action]")]
        public Task<IActionResult> Send([FromBody] string chatMessage)
        {
            var chat = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);
            return _commands.Execute(new SendMessage(chat.User, chat.Message, chat.Lobby), When<MessageSucceeded>.ThenOk, When<MessageFailed>.ThenBadRequest);
        }

        [HttpPost("[action]")]
        public Task<IActionResult> Create([FromBody] string newLobby)
        {
            var lobby = JsonConvert.DeserializeObject<ClientLobby>(newLobby);
            return _commands.Execute(new CreateLobby(lobby.LobbyId, lobby.User), When<LobbyCreated>.ThenOk, When<LobbyFailed>.ThenBadRequest);
        }
    }
}

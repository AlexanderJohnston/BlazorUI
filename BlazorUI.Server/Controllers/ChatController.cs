using BlazorUI.Client.Pages.Data;
using BlazorUI.Client.Queries;
using BlazorUI.Server.Attributes;
using BlazorUI.Shared.Events.Chat;
using BlazorUI.Shared.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost("[action]")]
        public Task<IActionResult> Send([FromBody] string chatMessage)
        {
            var chat = JsonConvert.DeserializeObject<ChatMessage>(chatMessage);
            return _commands.Execute(new SendMessage(chat.Message, chat.User), When<MessageSucceeded>.ThenOk, When<MessageFailed>.ThenBadRequest);
        }
    }
}

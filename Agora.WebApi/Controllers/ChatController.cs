using Agora.BLL.DTO;
using Agora.BLL.Storages;
using Agora.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            return Ok(ChatStorage.Messages);
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] Message message)
        {
            message.SentAt = DateTime.UtcNow;
            ChatStorage.Messages.Add(message);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearMessages()
        {
            ChatStorage.Messages.Clear();
            ChatStorage.HasClientStarted = false;
            await _hubContext.Clients.All.SendAsync("ChatCleared");
            await _hubContext.Clients.All.SendAsync("ChatStatusChanged", false);

            return Ok(new { message = "Chat cleared!" });
        }

        [HttpGet("can-admin-chat")]
        public IActionResult CanAdminChat()
        {
            return Ok(new { canChat = ChatStorage.HasClientStarted });
        }
    }
}

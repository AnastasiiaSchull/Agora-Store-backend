using Agora.BLL.DTO;
using Agora.BLL.Storages;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
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
        public IActionResult ClearMessages()
        {
            ChatStorage.Messages.Clear();
            return Ok("Chat cleared");
        }
    }
}

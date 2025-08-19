using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public FeedbackController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendFeedback([FromBody] FeedbackModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid feedback");

            try
            {
                string subject = "Feedback from Agora website";
                string body = $@"
                    <p><strong>Name:</strong> {model.Name}</p>
                    <p><strong>Email:</strong> {model.Email}</p>
                    <p><strong>Message:</strong><br>{model.Message}</p>";

                await _emailService.SendEmailAsync("agora.shop.company@gmail.com", subject, body);

                return Ok(new { message = "Feedback sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Failed to send feedback");
            }
        }
    }
}

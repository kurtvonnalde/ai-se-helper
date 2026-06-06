using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    [HttpPost("message")]
    public IActionResult SendMessage([FromBody] ChatRequest request)
    {
        var reply = $"You said: {request.Message}. (AI will reply here soon!)";
        return Ok(new { reply });
    }

}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
}

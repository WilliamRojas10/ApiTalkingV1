using Microsoft.AspNetCore.Mvc;

namespace ApiTalking.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> loguin()
    {
        
        return Ok();
    }
}
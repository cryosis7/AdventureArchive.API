using Microsoft.AspNetCore.Mvc;

namespace AdventureArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Hello 👋");
    }
}
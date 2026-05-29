using Microsoft.AspNetCore.Mvc;

namespace AIManHua.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComicController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateComic()
    {
        // Business logic will be implemented in the next phase
        return Ok(new { message = "Not implemented" });
    }

    [HttpGet("{id}")]
    public IActionResult GetComic(long id)
    {
        return Ok(new { message = "Not implemented" });
    }
}

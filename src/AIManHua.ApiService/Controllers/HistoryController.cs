using Microsoft.AspNetCore.Mvc;

namespace AIManHua.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHistory()
    {
        // Business logic will be implemented in the next phase
        return Ok(new { message = "Not implemented" });
    }
}

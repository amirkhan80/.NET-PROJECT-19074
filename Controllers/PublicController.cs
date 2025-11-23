using Microsoft.AspNetCore.Mvc;
using SmartServiceHub.Services;

namespace SmartServiceHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    private readonly IServiceTypeService _svc;

    public PublicController(IServiceTypeService svc)
    {
        _svc = svc;
    }

    // ✅ Get all service types for frontend
    [HttpGet("services")]
    public async Task<IActionResult> GetServiceTypes()
    {
        var list = await _svc.GetAllAsync();
        return Ok(list);
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartServiceHub.Services;

namespace SmartServiceHub.Controllers;
public class HomeController : Controller
{
    private readonly IServiceTypeService _serviceTypeService;
    public HomeController(IServiceTypeService serviceTypeService) { _serviceTypeService = serviceTypeService; }

    public async Task<IActionResult> Index()
    {
        var services = await _serviceTypeService.GetAllAsync();
        return View(services);
    }
}

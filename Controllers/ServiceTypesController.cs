using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartServiceHub.Models;
using SmartServiceHub.Services;

namespace SmartServiceHub.Controllers;

[Authorize(Policy = "AdminOnly")]
public class ServiceTypesController : Controller
{
    private readonly IServiceTypeService _svc;

    public ServiceTypesController(IServiceTypeService svc)
    {
        _svc = svc;
    }

    // ✅ SHOW ALL SERVICE TYPES
    public async Task<IActionResult> Index()
    {
        var list = await _svc.GetAllAsync();
        return View(list);
    }

    // ✅ CREATE PAGE
    public IActionResult Create()
    {
        return View();
    }

    // ✅ CREATE POST (Price + All Fields Save)
    [HttpPost]
    public async Task<IActionResult> Create(string name, string? description, decimal basePrice, int durationHours)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("", "Name is required");
            return View();
        }

        var service = new ServiceType
        {
            Name = name,
            Description = description,
            BasePrice = basePrice,
            DurationInHours = durationHours,
            Active = true
        };

        await _svc.CreateAsync(service);
        return RedirectToAction("Index");
    }

    // ✅ EDIT PAGE
    public async Task<IActionResult> Edit(string id)
    {
        var service = await _svc.GetByIdAsync(id);
        if (service == null)
            return NotFound();

        return View(service);
    }

    // ✅ EDIT POST (Price + All Fields Update)
    [HttpPost]
    public async Task<IActionResult> Edit(string id, string name, string? description, decimal basePrice, int durationHours)
    {
        var service = await _svc.GetByIdAsync(id);
        if (service == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("", "Name is required");
            return View(service);
        }

        service.Name = name;
        service.Description = description;
        service.BasePrice = basePrice;
        service.DurationInHours = durationHours;

        await _svc.UpdateAsync(service);
        return RedirectToAction("Index");
    }

    // ✅ DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _svc.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}

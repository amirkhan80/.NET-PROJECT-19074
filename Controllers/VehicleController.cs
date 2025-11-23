using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartServiceHub.Models;
using SmartServiceHub.Data;
using SmartServiceHub.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartServiceHub.Controllers
{
    public class VehicleController : Controller
    {
        private readonly MongoDbContext _db;
        private readonly IImageService _imageService;

        public VehicleController(MongoDbContext db, IImageService imageService)
        {
            _db = db;
            _imageService = imageService;
        }

        // ✅ List all vehicles of logged-in user
        public async Task<IActionResult> List()
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (uid == null) return RedirectToAction("Login", "Account");

            var vehicles = await _db.Vehicles
                .Find(v => v.UserId == uid)
                .SortByDescending(v => v.CreatedAt)
                .ToListAsync();

            return View(vehicles);
        }

        // ✅ GET - Add Vehicle Form
        [HttpGet]
        public IActionResult Create() => View();

        // ✅ POST - Create New Vehicle
        [HttpPost]
        public async Task<IActionResult> Create(string type, string makeModel, string regNumber, IFormFile? photo)
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (uid == null) return RedirectToAction("Login", "Account");

            string? imageUrl = null;
            if (photo != null)
            {
                imageUrl = await _imageService.UploadFileAsync(photo);
            }

            var vehicle = new Vehicle
            {
                UserId = uid,
                Type = type,
                MakeModel = makeModel,
                RegistrationNumber = regNumber,
                PhotoUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Vehicles.InsertOneAsync(vehicle);
            return RedirectToAction("List");
        }

        // ✅ GET - Edit Page
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var vehicle = await _db.Vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        // ✅ POST - Update Vehicle
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string type, string makeModel, string regNumber, IFormFile? photo)
        {
            var vehicle = await _db.Vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vehicle == null) return NotFound();

            vehicle.Type = type;
            vehicle.MakeModel = makeModel;
            vehicle.RegistrationNumber = regNumber;

            if (photo != null)
                vehicle.PhotoUrl = await _imageService.UploadFileAsync(photo);

            await _db.Vehicles.ReplaceOneAsync(v => v.Id == id, vehicle);

            return RedirectToAction("List");
        }

        // ✅ DELETE Vehicle
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _db.Vehicles.DeleteOneAsync(v => v.Id == id);
            return RedirectToAction("List");
        }

        // ✅ View Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var vehicle = await _db.Vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Models;
using SmartServiceHub.Services;
using System.Security.Claims;

namespace SmartServiceHub.Controllers
{
    public class BookingController : Controller
    {
        private readonly MongoDbContext _db;
        private readonly IBookingService _bookingService;
        private readonly IServiceTypeService _serviceTypeService;

        public BookingController(MongoDbContext db, IBookingService bookingService, IServiceTypeService serviceTypeService)
        {
            _db = db;
            _bookingService = bookingService;
            _serviceTypeService = serviceTypeService;
        }

        // ✅ Show booking form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var serviceTypes = await _serviceTypeService.GetAllAsync();
            if (serviceTypes == null || !serviceTypes.Any())
            {
                TempData["Error"] = "⚠️ No service types found in database. Please add some first.";
            }

            ViewBag.ServiceTypes = serviceTypes;
            return View();
        }

        // ✅ Handle form submit
        [HttpPost]
        public async Task<IActionResult> Create(string vehicleName, string serviceName, DateTime serviceDate)
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (uid == null)
                return RedirectToAction("Login", "Account");

            var serviceTypes = await _serviceTypeService.GetAllAsync();
            ViewBag.ServiceTypes = serviceTypes;

            if (string.IsNullOrWhiteSpace(vehicleName) || string.IsNullOrWhiteSpace(serviceName))
            {
                ModelState.AddModelError("", "Vehicle name and service type are required.");
                return View();
            }

            var service = await _db.ServiceTypes.Find(s => s.Name == serviceName).FirstOrDefaultAsync();
            decimal estimatedAmount = service?.BasePrice ?? 500;

            var vehicle = await _db.Vehicles.Find(v => v.Type == vehicleName).FirstOrDefaultAsync();
            string vehicleId = vehicle?.Id ?? ObjectId.GenerateNewId().ToString();

            var booking = new Booking
            {
                UserId = uid,
                VehicleId = vehicleId,
                ServiceTypeId = service?.Id ?? ObjectId.GenerateNewId().ToString(),
                ServiceDate = serviceDate,
                EstimatedAmount = estimatedAmount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _bookingService.CreateAsync(booking);
            TempData["Message"] = "✅ Booking created successfully!";
            return RedirectToAction("History");
        }

        // ✅ Show user bookings
        public async Task<IActionResult> History()
        {
            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (uid == null)
                return RedirectToAction("Login", "Account");

            var bookings = await _bookingService.GetUserBookingsAsync(uid);
            return View(bookings);
        }

        // ✅ Booking details
        public async Task<IActionResult> Details(string id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // ✅ Delete booking (POST)
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("History");

            await _db.Bookings.DeleteOneAsync(b => b.Id == id);
            TempData["Message"] = "🗑️ Booking deleted successfully!";
            return RedirectToAction("History");
        }
    }
}

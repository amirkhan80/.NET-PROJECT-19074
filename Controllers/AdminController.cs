using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Models;
using SmartServiceHub.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SmartServiceHub.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly MongoDbContext _db;
        private readonly IBookingService _bookingService;
        private readonly IServiceTypeService _serviceTypeService;

        public AdminController(MongoDbContext db, IBookingService bookingService, IServiceTypeService serviceTypeService)
        {
            _db = db;
            _bookingService = bookingService;
            _serviceTypeService = serviceTypeService;
        }

        // ✅ Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalBookings = await _db.Bookings.CountDocumentsAsync(Builders<Booking>.Filter.Empty);
            var pending = await _db.Bookings.CountDocumentsAsync(Builders<Booking>.Filter.Eq(b => b.Status, "Pending"));
            var completed = await _db.Bookings.CountDocumentsAsync(Builders<Booking>.Filter.Eq(b => b.Status, "Completed"));
            var users = await _db.Users.CountDocumentsAsync(Builders<User>.Filter.Empty);

            ViewBag.TotalBookings = totalBookings;
            ViewBag.Pending = pending;
            ViewBag.Completed = completed;
            ViewBag.Users = users;

            var bookings = await _bookingService.GetAllAsync();
            var userList = await _db.Users.Find(_ => true).ToListAsync();
            var vehicles = await _db.Vehicles.Find(_ => true).ToListAsync();
            var serviceTypes = await _db.ServiceTypes.Find(_ => true).ToListAsync();

            var enriched = from b in bookings
                           join u in userList on b.UserId equals u.Id into userGroup
                           from u in userGroup.DefaultIfEmpty()
                           join v in vehicles on b.VehicleId equals v.Id into vehicleGroup
                           from v in vehicleGroup.DefaultIfEmpty()
                           join s in serviceTypes on b.ServiceTypeId equals s.Id into serviceGroup
                           from s in serviceGroup.DefaultIfEmpty()
                           select new BookingDisplay
                           {
                               Id = b.Id,
                               UserName = u?.FullName ?? "Unknown User",
                               VehicleType = v?.Type ?? v?.MakeModel ?? "N/A",
                               ServiceType = s?.Name ?? "N/A",
                               ServiceDate = b.ServiceDate,
                               EstimatedAmount = b.EstimatedAmount,
                               Status = b.Status
                           };

            return View(enriched);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string bookingId, string status)
        {
            var b = await _bookingService.GetByIdAsync(bookingId);
            if (b == null) return NotFound();

            b.Status = status;
            await _bookingService.UpdateAsync(b);

            TempData["Message"] = $"✅ Booking {status} successfully.";
            return RedirectToAction("Dashboard");
        }
    }
}

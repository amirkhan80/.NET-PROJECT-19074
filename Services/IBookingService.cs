using SmartServiceHub.Models;

namespace SmartServiceHub.Services;
public interface IBookingService
{
    Task CreateAsync(Booking b);
    Task<List<Booking>> GetUserBookingsAsync(string userId);
    Task<List<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(string id);
    Task UpdateAsync(Booking b);
    Task<List<Booking>> FilterAsync(DateTime? from, DateTime? to, string? vehicleType, string? status);
}

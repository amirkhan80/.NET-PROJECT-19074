using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Models;

namespace SmartServiceHub.Services;
public class BookingService : IBookingService
{
    private readonly MongoDbContext _db;
    public BookingService(MongoDbContext db) { _db = db; }

    public async Task CreateAsync(Booking b) => await _db.Bookings.InsertOneAsync(b);

    public async Task<List<Booking>> GetUserBookingsAsync(string userId)
    {
        return await _db.Bookings.Find(b => b.UserId == userId)
                                 .SortByDescending(b => b.CreatedAt)
                                 .ToListAsync();
    }

    public async Task<List<Booking>> GetAllAsync() =>
        await _db.Bookings.Find(_ => true).SortByDescending(b => b.CreatedAt).ToListAsync();

    public async Task<Booking?> GetByIdAsync(string id) =>
        await _db.Bookings.Find(b => b.Id == id).FirstOrDefaultAsync();

    public async Task UpdateAsync(Booking b) =>
        await _db.Bookings.ReplaceOneAsync(x => x.Id == b.Id, b);

    public async Task<List<Booking>> FilterAsync(DateTime? from, DateTime? to, string? vehicleType, string? status)
    {
        var builder = Builders<Booking>.Filter;
        var filter = builder.Empty;
        if (from.HasValue) filter &= builder.Gte(b => b.ServiceDate, from.Value.Date);
        if (to.HasValue) filter &= builder.Lte(b => b.ServiceDate, to.Value.Date.AddDays(1).AddTicks(-1));
        if (!string.IsNullOrWhiteSpace(status)) filter &= builder.Eq(b => b.Status, status);

        var bookings = await _db.Bookings.Find(filter).SortByDescending(b => b.CreatedAt).ToListAsync();

        if (!string.IsNullOrWhiteSpace(vehicleType))
        {
            var vehicleIds = (await _db.Vehicles.Find(v => v.Type == vehicleType)
                               .Project(v => v.Id).ToListAsync()).ToHashSet();
            bookings = bookings.Where(b => vehicleIds.Contains(b.VehicleId)).ToList();
        }
        return bookings;
    }
}

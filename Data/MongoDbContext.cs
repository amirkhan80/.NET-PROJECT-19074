using MongoDB.Driver;
using SmartServiceHub.Models;

namespace SmartServiceHub.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        // ✅ Constructor for Dependency Injection
        public MongoDbContext(IMongoClient client, string databaseName)
        {
            _database = client.GetDatabase(databaseName);
        }

        // ✅ MongoDB Collections
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("Vehicles");
        public IMongoCollection<ServiceType> ServiceTypes => _database.GetCollection<ServiceType>("ServiceTypes");
        public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("Bookings");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");
    }
}

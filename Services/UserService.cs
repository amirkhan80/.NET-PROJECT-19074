using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Models;

namespace SmartServiceHub.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDbContext _db;
        public UserService(MongoDbContext db) { _db = db; }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetByIdAsync(string id) =>
            await _db.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _db.Users.InsertOneAsync(user);

        public async Task<List<User>> GetAllAsync() =>
            await _db.Users.Find(_ => true).SortByDescending(u => u.CreatedAt).ToListAsync();

        public async Task UpdateAsync(User user) =>
            await _db.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }
}

using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Models;

namespace SmartServiceHub.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly MongoDbContext _db;

        public ServiceTypeService(MongoDbContext db)
        {
            _db = db;
        }

        public async Task<List<ServiceType>> GetAllAsync() =>
            await _db.ServiceTypes
                .Find(_ => true)
                .SortBy(s => s.Name)
                .ToListAsync();

        public async Task<ServiceType?> GetByIdAsync(string id) =>
            await _db.ServiceTypes
                .Find(s => s.Id == id)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(ServiceType s) =>
            await _db.ServiceTypes.InsertOneAsync(s);

        public async Task UpdateAsync(ServiceType s) =>
            await _db.ServiceTypes.ReplaceOneAsync(x => x.Id == s.Id, s);

        public async Task DeleteAsync(string id) =>
            await _db.ServiceTypes.DeleteOneAsync(s => s.Id == id);
    }
}

using SmartServiceHub.Data;
using SmartServiceHub.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace SmartServiceHub.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MongoDbContext _db;

        public PaymentService(MongoDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task CreateAsync(Payment p)
        {
            await _db.Payments.InsertOneAsync(p);
        }

        public async Task<Payment?> GetByIdAsync(string id)
        {
            return await _db.Payments.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}

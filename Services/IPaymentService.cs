using SmartServiceHub.Models;
using System.Threading.Tasks;

namespace SmartServiceHub.Services
{
    public interface IPaymentService
    {
        Task CreateAsync(Payment p);
        Task<Payment?> GetByIdAsync(string id);
    }
}

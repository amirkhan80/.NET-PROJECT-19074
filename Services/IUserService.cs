using SmartServiceHub.Models;

namespace SmartServiceHub.Services;
public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
    Task CreateAsync(User user);
    Task<List<User>> GetAllAsync();
    Task UpdateAsync(User user);
}

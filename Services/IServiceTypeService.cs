using SmartServiceHub.Models;

namespace SmartServiceHub.Services;
public interface IServiceTypeService
{
    Task<List<ServiceType>> GetAllAsync();
    Task<ServiceType?> GetByIdAsync(string id);
    Task CreateAsync(ServiceType s);
    Task UpdateAsync(ServiceType s);
    Task DeleteAsync(string id);
}

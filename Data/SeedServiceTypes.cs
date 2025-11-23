using SmartServiceHub.Models;
using SmartServiceHub.Services;

namespace SmartServiceHub.Data
{
    public class SeedServiceTypes
    {
        public static async Task SeedAsync(IServiceTypeService svc)
        {
            var existing = await svc.GetAllAsync();
            if (existing.Count > 0) return;

            var list = new List<ServiceType>
            {
                new ServiceType { Name = "Basic Bike Service", BasePrice = 499 },
                new ServiceType { Name = "Full Bike Service", BasePrice = 999 },
                new ServiceType { Name = "Engine Oil Change", BasePrice = 299 },
                new ServiceType { Name = "Car Wash & Cleaning", BasePrice = 699 },
                new ServiceType { Name = "Car General Service", BasePrice = 2499 },
                new ServiceType { Name = "Brake Pad Replacement", BasePrice = 799 },
                new ServiceType { Name = "Chain Lubrication", BasePrice = 149 },
                new ServiceType { Name = "Tyre Replacement", BasePrice = 1200 },
            };

            foreach (var s in list)
                await svc.CreateAsync(s);
        }
    }
}

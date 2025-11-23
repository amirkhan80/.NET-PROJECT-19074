namespace SmartServiceHub.Models;

public class VehicleViewModel
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? Type { get; set; }
    public string? MakeModel { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

namespace SmartServiceHub.Models
{
    public class BookingViewModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? VehicleId { get; set; }
        public string? ServiceTypeId { get; set; }

        // Vehicle Info
        public string? Type { get; set; }
        public string? MakeModel { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? PhotoUrl { get; set; }

        // Booking Info
        public DateTime? ServiceDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending";
        public decimal? EstimatedAmount { get; set; }
        public decimal? FinalAmount { get; set; }

        // Service Info
        public string? ServiceName { get; set; }

        // Optional
        public string? Notes { get; set; }

        // ✅ Added for Details view
        public string? CustomerName { get; set; }        // fix 1
        public string? VehicleNumber => RegistrationNumber; // fix 2
        public string? ServiceTypeName => ServiceName;      // fix 3
    }
}

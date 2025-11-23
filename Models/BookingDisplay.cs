using System;

namespace SmartServiceHub.Models
{
    public class BookingDisplay
    {
        public string Id { get; set; } = "";
        public string UserName { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public DateTime ServiceDate { get; set; }
        public decimal EstimatedAmount { get; set; }
        public string Status { get; set; } = "";
    }
}

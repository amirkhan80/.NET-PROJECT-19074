using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartServiceHub.Models;
public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string VehicleId { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string ServiceTypeId { get; set; } = null!;
    public DateTime ServiceDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending"; // Pending|Accepted|InProgress|Completed|Cancelled
    public decimal EstimatedAmount { get; set; }
    public decimal? FinalAmount { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? AssignedMechanicId { get; set; }
    public string? Notes { get; set; }
}

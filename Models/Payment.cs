using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartServiceHub.Models;
public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string BookingId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = "";
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string? TransactionRef { get; set; }
}

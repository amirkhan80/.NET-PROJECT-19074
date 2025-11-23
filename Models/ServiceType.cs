using MongoDB.Bson;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartServiceHub.Models
{
    public class ServiceType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // ✅ Service Name
        public string Name { get; set; } = "";
        public string? VehicleCategory { get; set; }


        // ✅ Optional Description
        public string? Description { get; set; }

        // ✅ Price (BasePrice used by controller)
        public decimal BasePrice { get; set; }

        // ✅ Time needed for service (in hours)
        public int DurationInHours { get; set; }

        // ✅ Whether service is active or disabled
        public bool Active { get; set; }
    }
}

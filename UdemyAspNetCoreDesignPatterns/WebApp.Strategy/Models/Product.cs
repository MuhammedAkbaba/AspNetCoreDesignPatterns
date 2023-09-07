using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Strategy.Models
{
    public class Product
    {
        [BsonId]
        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public string UserId { get; set; }


        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreateDate { get; set; }

    }
}

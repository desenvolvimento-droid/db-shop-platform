using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shop.Persistence.ReadModels
{
    public class CustomerReadModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
        public string Update { get; set; } = default!;
    }
}

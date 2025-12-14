using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shop.Application.ReadModels.Base;

namespace Shop.Persistence.ReadModels;

public class ProductReadModel : ReadModelBase
{
    public string Name { get; set; } = default!;

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }
}

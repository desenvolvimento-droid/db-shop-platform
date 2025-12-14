using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shop.Application.ReadModels.Base;

namespace Shop.Persistence.ReadModels
{
    public class CustomerReadModel : ReadModelBase
    {
        public string Name { get; set; } = default!;
        public string Update { get; set; } = default!;
    }
}

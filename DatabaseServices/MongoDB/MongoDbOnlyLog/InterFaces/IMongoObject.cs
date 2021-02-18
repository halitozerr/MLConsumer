using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MLConsumer.DatabaseServices.MongoDB.InterFaces
{
   public interface IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
    }
}

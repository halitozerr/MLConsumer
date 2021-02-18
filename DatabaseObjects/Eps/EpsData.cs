using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MLConsumer.DatabaseObjects.Eps
{
    public class EpsData : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Date { get; set; }
        public int EpsCalculate { get; set; }
    }
}

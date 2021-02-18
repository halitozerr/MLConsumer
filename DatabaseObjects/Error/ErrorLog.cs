using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MLConsumer.DatabaseObjects.Error
{
    public class ErrorLog : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Date { get; set; }
        public string Level { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorMessage { get; set; }
    }
}

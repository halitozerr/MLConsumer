using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MLConsumer.DatabaseObjects.UsedDevices
{
    public class UsedDevice : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceClassName { get; set; }
    }
}

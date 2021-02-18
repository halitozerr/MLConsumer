using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MLConsumer.DatabaseObjects.UnRegisteredDevices
{
    public class UnRegisteredDevice : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public string IPAdress { get; set; }
        public bool isActive { get; set; }
    }

}

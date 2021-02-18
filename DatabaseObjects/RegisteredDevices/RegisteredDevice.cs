using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MLConsumer.DatabaseObjects.RegisteredDevices
{
    public class RegisteredDevice : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceDescription { get; set; }
        public string DeviceParseMethod { get; set; }
        public string IPAdress { get; set; }
        public string CollectionName { get; set; }
        public List<string> RegexStatements { get; set; }
        public bool isActive { get; set; }

    }
}

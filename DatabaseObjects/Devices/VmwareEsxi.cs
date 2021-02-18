using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MLConsumer.DatabaseObjects.Devices
{
    class VmwareEsxi : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime date { get; set; }
        public string devName { get; set; }
        public string type { get; set; }
        public string typeInfo { get; set; }
        public string process { get; set; }
        public string orgn { get; set; }
        public string sub { get; set; }
        public string msg { get; set; }

    }
}

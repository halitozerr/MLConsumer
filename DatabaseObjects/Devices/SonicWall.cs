using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MLConsumer.DatabaseObjects.Devices
{
    public class SonicWall : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string logid { get; set; }
        public string sn { get; set; }
        public DateTime time { get; set; }
        public string fw { get; set; }
        public string pri { get; set; }
        public string c { get; set; }
        public string m { get; set; }
        public string msg { get; set; }
        public string app { get; set; }
        public string n { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public string dstMac { get; set; }
        public string proto { get; set; }
        public string sent { get; set; }
        public string dpi { get; set; }
        public string rule { get; set; }
        public string fw_action { get; set; }
    }
}

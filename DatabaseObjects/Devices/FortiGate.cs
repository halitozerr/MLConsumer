using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Net;

namespace MLConsumer.DatabaseObjects.Devices
{
   public class FortiGate: IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string date { get; set; }
        public string devname { get; set; }
        public string devid { get; set; }
        public string eventtime { get; set; }
        public string tz { get; set; }
        public string logid { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string eventtype { get; set; }
        public string level { get; set; }
        public string vd { get; set; }
        public string appid { get; set; }
        public string countapp { get; set; }
        public string srcip { get; set; }
        public string srcport { get; set; }
        public string srcintf { get; set; }
        public string srcname { get; set; }
        public string srcintfrole { get; set; }
        public string devtype { get; set; }
        public string osname { get; set; }
        public string mastersrcmac { get; set; }
        public string srcmac { get; set; }
        public string transport { get; set; }
        public string transip { get; set; }
        public string dstip { get; set; }
        public string dstport { get; set; }
        public string dstintf { get; set; }
        public string dstname { get; set; }
        public string dstintfrole { get; set; }
        public string srccountry { get; set; }
        public string dstcountry { get; set; }
        public string sessionid { get; set; }
        public string proto { get; set; }
        public string action { get; set; }
        public string policyid { get; set; }
        public string applist { get; set; }
        public string policytype { get; set; }
        public string policymode { get; set; }
        public string poluuid { get; set; }
        public string policyname { get; set; }
        public string service { get; set; }
        public string direction { get; set; }
        public string trandisp { get; set; }
        public string duration { get; set; }
        public string sentbyte { get; set; }
        public string rcvdbyte { get; set; }
        public string sentpkt { get; set; }
        public string rcvdpkt { get; set; }
        public string vpn { get; set; }
        public string vpntype { get; set; }
        public string appcat { get; set; }
        public string app { get; set; }
        public string incidentserialno { get; set; }
        public string utmaction { get; set; }
        public string utmref { get; set; }
        public string msg { get; set; }
        public string apprisk { get; set; }




    }
}

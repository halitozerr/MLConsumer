using MLConsumer.DatabaseServices.MongoDB.InterFaces;

namespace MLConsumer.DatabaseServices.MongoDB
{
    public class LogDatabaseSettings : ILogDatabaseSettings
    {
        public string CollectionName { get; set; }  
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

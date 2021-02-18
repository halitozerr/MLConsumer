using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;

namespace MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure
{
    public class DatabaseSettings<T> : IDatabaseSettings<T>
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

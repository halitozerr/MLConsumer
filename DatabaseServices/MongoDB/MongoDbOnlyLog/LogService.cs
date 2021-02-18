using MongoDB.Driver;
using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using System.Collections.Generic;
using System.Linq;
namespace MLConsumer.DatabaseServices.MongoDB
{
    public class LogService<T> : ILogService<T> where T : IMongoObject
    {
        private IMongoCollection<T> _logs;
        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        public LogService(ILogDatabaseSettings settings)
        {
            //var mongoClientSettings = new MongoClientSettings();
            //mongoClientSettings.Compressors = new List<CompressorConfiguration> { new CompressorConfiguration(Mongodb) };
            client = new MongoClient(settings.ConnectionString);

            database = client.GetDatabase(settings.DatabaseName);
        }

        public void CreateConnection(string CollectionName)
        {
            _logs = database.GetCollection<T>(CollectionName);
        }
        public T Create(T book)
        {
            _logs.InsertOneAsync(book);
            return book;
        }
        public List<T> Get()
        {
            return _logs.Find(collection => true).ToList();
        }
        //public List<T> GetPage(int pageNumber, int nPerPage,string ownedDeviceId)
        //{
        //    return _logs.Find(collection => true && collection.ownedDeviceId == ownedDeviceId).Skip(pageNumber > 0 ? ((pageNumber - 1) * nPerPage) : 0).Limit(nPerPage).ToList();
        //}

    }
}

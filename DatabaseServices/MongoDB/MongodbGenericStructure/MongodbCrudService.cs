using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;

namespace MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure
{
    public class MongodbCrudService<T> : IDatabaseService<T> where T : IMongoObject
    {
        private IMongoCollection<T> _collections;
        public MongodbCrudService(IDatabaseSettings<T> settings)
        {
            Reconnect(settings);     
        }

        private void Reconnect(IDatabaseSettings<T> settings)
        {
            var mres = new ManualResetEventSlim(false); // state is initially false         
            while (!mres.Wait(3000)) // loop until state is true, checking every 3s
            {
                try
                {
                    Connect(settings);
              
                    mres.Set();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MongoDB is not working");
                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                    Environment.Exit(0);
                }
            }
        }
        private void Connect(IDatabaseSettings<T> settings)
        {
            try
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);
                _collections = database.GetCollection<T>(settings.CollectionName);
            }
            catch (Exception)
            {

                throw;
            }


        }
        public T Create(T value)
        {            
            _collections.InsertOne(value);
            return value;
        }

        public List<T> Get()
        {
            var a =_collections.Find(collection => true).ToList();
            return a;
        }

        public T Get(string id)
        {
            return _collections.Find(collection => collection.Id == id).FirstOrDefault();
        }

   

        public List<T> GetPage(int pageNumber, int nPerPage)
        {
            return _collections.Find(collection => true).Skip(pageNumber > 0 ? ((pageNumber - 1) * nPerPage) : 0).Limit(nPerPage).ToList();
        }

        public int GetPageCount(int nPerPage)
        {
            var pageCount = _collections.Find(collection => true).ToList().Count() / nPerPage;
            return pageCount;
        }

        public void Remove(T value)
        {
            _collections.DeleteOne(collection => collection.Id == value.Id);
        }

        public void Remove(string id)
        {
            _collections.DeleteOne(collection => collection.Id == id);
        }

        public void Update(string id, T objectIn)
        {
            _collections.ReplaceOne(collection => collection.Id == id, objectIn);
        }
    }
}

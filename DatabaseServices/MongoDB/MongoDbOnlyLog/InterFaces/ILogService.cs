using System.Collections.Generic;
namespace MLConsumer.DatabaseServices.MongoDB.InterFaces
{
    public interface ILogService<T>
    {
        public T Create(T value);
        public List<T> Get();
        public void CreateConnection(string CollectionName);
        //public List<T> GetPage(int pageNumber, int nPerPage, string ownedDeviceId);
    }
}

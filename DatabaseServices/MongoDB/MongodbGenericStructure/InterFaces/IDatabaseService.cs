using System.Collections.Generic;
namespace MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces
{
   public interface IDatabaseService<T>
    {
        public List<T> Get();

        public T Get(string id);
        public List<T> GetPage(int pageNumber, int nPerPage);
        public int GetPageCount(int nPerPage);

        public T Create(T value);

        public void Update(string id, T logIn);

        public void Remove(T logIn);

        public void Remove(string id);
    }
}

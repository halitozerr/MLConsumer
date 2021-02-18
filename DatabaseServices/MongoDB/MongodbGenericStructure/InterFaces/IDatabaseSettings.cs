namespace MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces
{
    public interface IDatabaseSettings<T>
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

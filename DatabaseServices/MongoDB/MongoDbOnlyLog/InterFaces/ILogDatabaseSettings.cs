namespace MLConsumer.DatabaseServices.MongoDB.InterFaces
{
    public interface ILogDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

namespace ApiLogging.EntitiesCQ.Mongo.Interface
{
    public interface IMongoRepository
    {
        public Task<T> InsertRecordAsync<T>(string table, T record);
        public Task<List<T>> LoadRecordsAsync<T>(string table);
        public Task<T> LoadRecordByIdAsync<T>(string table, Guid id);
        public Task UpsertRecordAsync<T>(string table, Guid id, T record);
        public Task DeleteRecordAsync<T>(string table, Guid id);
    }
}

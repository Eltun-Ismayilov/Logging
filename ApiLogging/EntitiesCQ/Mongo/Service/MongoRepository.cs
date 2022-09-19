using ApiLogging.EntitiesCQ.Mongo.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiLogging.EntitiesCQ.Mongo.Service
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public MongoRepository(IConfiguration configuration)
        {
            var connectionstring = configuration["MongoDb:ConnectionString"];
            var dbName = configuration["MongoDb:DatabaseName"];
            var client = new MongoClient(connectionstring);
            _database = client.GetDatabase(dbName);
        }

        public async Task<T> InsertRecordAsync<T>(string table, T record)
        {
            var collection = _database.GetCollection<T>(table);
            await collection.InsertOneAsync(record);
            return record;
        }
        public async Task<List<T>> LoadRecordsAsync<T>(string table)
        {
            var collection = _database.GetCollection<T>(table);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task<T> LoadRecordByIdAsync<T>(string table, Guid id)
        {
            var collection = _database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return (await collection.FindAsync(filter)).First();
        }
        public async Task UpsertRecordAsync<T>(string table, Guid id, T record)
        {
            var collection = _database.GetCollection<T>(table);
            var result = await collection.ReplaceOneAsync(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true });
        }
        public async Task DeleteRecordAsync<T>(string table, Guid id)
        {
            var collection = _database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.DeleteOneAsync(filter);
        }
    }
}

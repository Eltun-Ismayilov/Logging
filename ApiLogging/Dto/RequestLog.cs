using MongoDB.Bson.Serialization.Attributes;

namespace ApiLogging.Dto
{
    public class RequestLog
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement]
        public string RequestUrl { get; set; }
        [BsonElement]
        public string RequestHeaders { get; set; }
        [BsonElement]
        public int ResponseStatusCode { get; set; }
        [BsonElement]
        public string ResponseBody { get; set; }
        [BsonElement]
        public string ResponseHeaders { get; set; }
        [BsonElement]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        [BsonElement]
        public string? RequestIP { get; set; }
    }
}

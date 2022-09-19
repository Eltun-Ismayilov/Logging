using ApiLogging.Dto;

namespace ApiLogging.EntitiesCQ.Interface
{
    public interface ILoggingService
    {
        Task<RequestLog> SaveRequestLog(HttpRequest request);
        Task SaveResponseLog(HttpResponse response, RequestLog requestLog);
    }
}

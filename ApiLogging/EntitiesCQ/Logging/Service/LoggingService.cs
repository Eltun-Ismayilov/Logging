using ApiLogging.Dto;
using ApiLogging.EntitiesCQ.Interface;
using ApiLogging.EntitiesCQ.Mongo.Interface;
using System.Text;

namespace ApiLogging.EntitiesCQ.Service
{
    public class LoggingService : ILoggingService
    {
        private readonly string _requestLogCollection;

        private readonly IMongoRepository _mongoRepository;

        public LoggingService(IConfiguration configuration, IMongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
            _requestLogCollection = configuration["MongoDb:RequestLogCollection"];
        }

        public async Task<RequestLog> SaveRequestLog(HttpRequest request)
        {
            RequestLog requestLog = new();

            request.EnableBuffering();
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";
            request.Body.Position = 0;

            requestLog.RequestUrl = formattedRequest;

            var headers = new StringBuilder();
            foreach (var header in request.Headers)
            {
                headers.Append(header.Key).Append(':').AppendLine(header.Value);
            }

            requestLog.RequestHeaders = headers.ToString();
            requestLog.RequestIP = request.HttpContext.Connection.RemoteIpAddress?.ToString();

            return await _mongoRepository.InsertRecordAsync(_requestLogCollection, requestLog);
        }

        public async Task SaveResponseLog(HttpResponse response, RequestLog requestLog)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            requestLog.ResponseStatusCode = response.StatusCode;
            requestLog.ResponseBody = text;

            var headers = new StringBuilder();
            foreach (var header in response.Headers)
            {
                headers.Append(header.Key).Append(':').AppendLine(header.Value);
            }

            requestLog.ResponseHeaders = headers.ToString();
            await _mongoRepository.UpsertRecordAsync(_requestLogCollection, requestLog.Id, requestLog);
        }
    }
}

using ApiLogging.EntitiesCQ.Interface;

namespace ApiLogging.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggingService loggingService)
        {
            var requestLog = await loggingService.SaveRequestLog(context.Request);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            await loggingService.SaveResponseLog(context.Response, requestLog);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MyBackend.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log request
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            _logger.LogInformation("Incoming Request: {method} {url} | Headers: {@headers} | Body: {body}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Request?.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                string.IsNullOrWhiteSpace(requestBody) ? "N/A" : requestBody);

            // Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {method} {url}",
                    context.Request?.Method,
                    context.Request?.Path.Value);
                throw;
            }

            // Log response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Outgoing Response: {statusCode} | Body: {body}",
                context.Response?.StatusCode,
                string.IsNullOrWhiteSpace(responseText) ? "N/A" : responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

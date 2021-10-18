using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductmanagementCore.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace ProductmanagementCore.Common
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception: {0} , {1}", ex.Message, ex.StackTrace));
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            switch (exception)
            {
                case WebException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ValidationException _:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            var errorHttpObject = new ErrorHttp
            {
                Result = new { Msg = exception.Message, Trace = exception.StackTrace },
                Code = (int)code,
                Message = "Critical Exception"
            };
            var result = JsonConvert.SerializeObject(errorHttpObject);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

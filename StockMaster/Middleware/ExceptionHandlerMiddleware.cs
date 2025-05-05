using Microsoft.AspNetCore.Http.HttpResults;
using StockMaster.Dtos.Error;
using StockMaster.Exceptions;
using System.Net;

namespace StockMaster.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var error = new ErrorDto
            {
                Message = ex.Message,
                ExceptionType = ex.GetType().Name,
                StatusCode = ex switch
                {
                    NotFoundException => HttpStatusCode.NotFound,
                    UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    HttpRequestException => HttpStatusCode.BadRequest,
                    ArgumentException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                },
                StackTrace = _env.IsDevelopment() ? ex.StackTrace : null

            };

            _logger.LogError(error.ToString());

            await context.Response.WriteAsJsonAsync(error);


            
        }
    }
}

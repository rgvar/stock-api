using System.Net;
using System.Text.Json;

namespace StockMaster.Dtos.Error
{
    public class ErrorDto
    {
        public Guid ErrorId { get; set; } = Guid.NewGuid();
        public string? Message { get; set; }
        public string? ExceptionType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? StackTrace { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

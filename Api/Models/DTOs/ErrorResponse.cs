namespace Api.Models.DTOs
{
    public class ErrorResponse
    {
        public ApiResponseStatus Status { get; set; }
        public string Message { get; set; } = default!;
        public int StatusCode { get; set; }
        public string? InformationLink { get; set; }
        public object? Details { get; set; }
        public string? TraceId { get; set; }
    }
}

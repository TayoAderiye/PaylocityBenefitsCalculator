using System.Diagnostics;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using Api.Models;
using Api.Models.DTOs;
using Api.Models.Exceptions;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorResponse();
            //var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production";
            var tracingId = GetTracingId(context);
            errorResponse.TraceId = tracingId;

            switch (ex)
            {
                case DbInitislizerException e:
                    errorResponse.Status = ApiResponseStatus.Fail;
                    errorResponse.StatusCode = e.StatusCode;
                    errorResponse.Message = e.Message;
                    errorResponse.InformationLink =  e.HelpLink;
                    errorResponse.Details = e.Error ?? e.InnerException?.Data?.Values ?? e.Data?.Values;
                    context.Response.StatusCode = e.StatusCode;
                    break;
                default:
                    errorResponse.Status = ApiResponseStatus.Error;
                    const int code = (int)HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = code;
                    errorResponse.InformationLink = ex.HelpLink;
                    errorResponse.Message = ex.Message;
                    errorResponse.Details = ex.Data.Values;
                    context.Response.StatusCode = code;
                    var stackTrace = new StackTrace(ex, true);
                    break;
            }

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var result = JsonSerializer.Serialize(errorResponse, jsonSerializerOptions);
            await context.Response.WriteAsync(result);
        }


        private static string GetTracingId(HttpContext context)
        {
            var isClientTraceId = context.Request.Headers.ContainsKey("X-Moo-Trace-Id");
            return isClientTraceId
                    ? context.Request.Headers["X-Moo-Trace-Id"].ToString()
                    : Guid.NewGuid().ToString();
        }

        private static string FormatStackTrace(StackTrace stackTrace)
        {
            var frame = stackTrace.GetFrame(0);
            if (frame is null)
            {
                return "";
            }

            var fileName = frame.GetFileName();
            var methodName = frame.GetMethod()!.Name;
            var line = frame.GetFileLineNumber();
            var column = frame.GetFileColumnNumber();

            return $"Source: {methodName} ({fileName}, line {line}, column {column})";
        }
    }
}

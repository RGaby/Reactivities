using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate m_next;
        private readonly ILogger<ExceptionMiddleware> m_logger;
        private readonly IHostEnvironment m_environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.m_environment = environment;
            this.m_logger = logger;
            this.m_next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await m_next(context);
            }
            catch (System.Exception ex)
            {
                m_logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = m_environment.IsDevelopment() ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace) :
                new AppException(context.Response.StatusCode, "Internal server error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
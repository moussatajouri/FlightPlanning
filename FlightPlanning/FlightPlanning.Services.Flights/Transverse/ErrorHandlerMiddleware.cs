using FlightPlanning.Services.Flights.Transverse.Exception;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception exception)
            {
                await HandleErrorAsync(context, exception);
            }
        }

        private static Task HandleErrorAsync(HttpContext context, System.Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            
            var code = "error";
            var message = "There was an error.";
            var type = "unhandled";
            var innerExceptionMessage = string.Empty;
            var innerExceptionStackTrace = string.Empty;

            switch (exception)
            {
                case FlightPlanningFunctionalException e:
                    code = e.Code;
                    message = e.Message;
                    type = "functional";
                    statusCode = HttpStatusCode.BadRequest;                    
                    break;
                case FlightPlanningTechnicalException e:
                    code = e.Code;
                    message = e.Message;
                    innerExceptionMessage = e.InnerException?.Message;
                    innerExceptionStackTrace = e.InnerException?.StackTrace;
                    type = "technical";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new
            {
                code,
                message,
                type,
                innerExceptionMessage,
                innerExceptionStackTrace
            };

            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}

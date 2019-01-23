using FlightPlanning.Services.Flights.Transverse.Exception;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse.Exception
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

            var response = new Anomaly
            {
                Code = "error",
                Message = "There was an error.",
                Type = "unhandled"
            };

            switch (exception)
            {
                case FlightPlanningFunctionalException e:
                    response.Code = e.Code;
                    response.Message = e.Message;
                    response.Type = "functional";
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case FlightPlanningTechnicalException e:
                    response.Code = e.Code;
                    response.Message = e.Message;
                    response.InnerExceptionMessage = e.InnerException?.Message;
                    response.InnerExceptionStackTrace = e.InnerException?.StackTrace;
                    response.Type = "technical";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}

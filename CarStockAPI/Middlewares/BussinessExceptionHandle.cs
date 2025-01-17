using System;
using System.Net;
using CarStockBLL.CustomException;
using Newtonsoft.Json;

namespace CarStockAPI.Middlewares
{
    public class BussinessExceptionHandle : AbstractExceptonMiddleware
    {
        public BussinessExceptionHandle(
            ILogger<AbstractExceptonMiddleware> logger,
            RequestDelegate next)
                : base(logger, next) { }

        public override (HttpStatusCode code, string message) GetException(ApiException apiException)
        {
            HttpStatusCode code;
            switch (apiException)
            {
                case ApiException:
                    code = HttpStatusCode.NotFound;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }
            var response = new
            {
                Error = apiException.Message,
                Type = apiException.GetType().Name,
                Details = apiException.Message ?? "An error occurred while processing your request"
            };

            return (code, JsonConvert.SerializeObject(response));
        }
    }
}

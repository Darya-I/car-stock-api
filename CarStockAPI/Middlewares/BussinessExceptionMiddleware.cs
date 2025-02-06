using System.Net;
using CarStockBLL.CustomException;


namespace CarStockAPI.Middlewares
{
    /// <summary>
    /// Middleware для обработки бизнес-исключений и формирования стандартного HTTP-ответа с кодом и сообщением об ошибке
    /// </summary>
    public class BussinessExceptionMiddleware : AbstractExcepton
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BussinessExceptionMiddleware"/>.
        /// </summary>
        /// <param name="next">Делегат, передающий управление следующему middleware.</param>
        /// <param name="logger">Экземпляр логгера</param>
        public BussinessExceptionMiddleware(
            RequestDelegate next,
            ILogger<AbstractExcepton> logger)
            : base(next, logger) { }

        /// <summary>
        /// Определяет HTTP-статус код и сообщение для указанного исключения
        /// </summary>
        /// <param name="exception">Исключение, которое нужно обработать</param>
        /// <returns>Кортеж с HTTP-статус кодом и сериализованным сообщением об ошибке</returns>
        public override (HttpStatusCode code, object response) GetException(Exception exception)
        {
            HttpStatusCode code;
            switch (exception)
            {
                case InvalidUserDataException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ValidationErrorException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case EntityAlreadyExistsException:
                    code = HttpStatusCode.Conflict;
                    break;
                case EntityNotFoundException: 
                    code = HttpStatusCode.NotFound; 
                    break;
                case ApiException:
                    code = HttpStatusCode.InternalServerError;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }
            var response = new
            {
                Error = exception.Message,
                Type = exception.GetType().Name,
                Details = exception.Message ?? "An error occurred while processing your request"
            };

            return (code, response);
        }
    }
}

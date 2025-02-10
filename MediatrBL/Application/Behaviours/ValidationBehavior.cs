using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MediatrBL.Application.Behaviours
{
    /// <summary>
    /// Поведение для валидации команд в конвейере MediatR.
    /// Проверяет команду на наличие ошибок перед её выполнением
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса </typeparam>
    /// <typeparam name="TResponse">Тип ответа на запрос</typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Коллекция валидаторов для валидации запросов
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger,
                                  IEnumerable<IValidator<TRequest>> validators) 
        {
            _logger = logger;
            _validators = validators;
        }

        /// <summary>
        /// Обрабатывает запрос, выполняет валидацию перед передачей следующему обработчику
        /// </summary>
        /// <param name="request">Запрос, который необходимо обработать</param>
        /// <param name="next">Делегат следующего обработчика в конвейере</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на запрос</returns>
        /// <exception cref="ValidationException">Бросает исключение, если валидация не прошла</exception>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any()) 
            {
                string typeName = request.GetType().ToString();

                _logger.LogInformation("Validating command {CommandType}", typeName);

                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

                var failures = validationResults
                    .Where(r => r.Errors.Count > 0)
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any()) 
                {
                    _logger.LogWarning(
                       "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}",
                       typeName, request, failures);

                    throw new ValidationException(failures);
                }
            }

            return await next().ConfigureAwait(false);
        }
    }
}

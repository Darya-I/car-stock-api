using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using MediatrBL.Application.Behaviours;
using MediatrBL.Application.Handlers.Cars;
using MediatrBL.Application.Handlers.Users;

namespace CarStockAPI.DependencyInjection
{
    /// <summary>
    /// Модуль для регистрации MediatR в контейнере Autofac
    /// </summary>
    public class MediatorModule : Autofac.Module
    {
        /// <summary>
        /// Выполняет регистрацию компонентов в контейнере
        /// </summary>
        /// <param name="builder">Контейнер Autofac для регистрации зависимостей</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Получаем сборку с валидаторами и обработчиками
            var assembly = typeof(RegisterUserCommandHandler).Assembly;

            // Должно регистрироваться отдельно т.к. IRequestHandler имеет две версии
            var deleteCarCommandAssembly = typeof(DeleteCarCommandHandler).Assembly;

            builder.RegisterAssemblyTypes(deleteCarCommandAssembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<>))
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces();

            // Регистрация всех валидаторов
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces();

            // Регистрация всех обработчиков команд/запросов
            builder.RegisterAssemblyTypes(assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces();

            // Регистрация ValidationBehavior в MediatR pipeline
            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>));

            // Регистрация MediatR
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                   .AsImplementedInterfaces();
        }
    }
}

// https://learn.microsoft.com/ru-ru/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr
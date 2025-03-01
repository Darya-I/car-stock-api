# Car stock API
.NET 6.0 RESTful Справочник автомобилей. Трехуровневая архитектура, CRUD, аутентификация JWT Bearer, Google OAuth 2.0.

![https://badgen.net/badge/status/achieved?color=purple](https://badgen.net/badge/status/achieved?color=purple)

## DAL
Взаимодействует с БД и передает результат слою выше.
- PostgreSQL
- Entity Framework
- Microsoft Identity

### Гибридное управление пользователями:
- Модель `User` наследуется от `IdentityUser`, добавлены поля для работы с refresh token и ролью
- Кастомные роли, изменена связь на один-ко-многим. В модели `Role` определены возможности роли

### Паттерн репозиторий
Для основных сущностей реализованы интерфейсы и классы, выполняющие основную логику, реализуют `BaseRepository` с общим методом.

## BLL
Основная логика работы приложения, обработка исключений, DTO
- Mapperly
- JWT Bearer

### Аутентификация пользователей
`AuthorizeUserService` обращается к `UserSerivce` и `TokenSerivce`, в access токен помещаются политики из роли пользователя, передает результат в API; 
обновляет refresh токен пользователя в БД.
 
 ### Custom Exception
Производные от кастомного `ApiException` классы обрабатывают сценарии:
 - сущность не найдена,
 - сущность уже существует,
 - данные от пользователя не валидны

В остальных случаях выбрасываются `ApiException` и `Exception`. Все исключения перебрасываются в middleware на уровень выше.

## Web API
Организованы эндпоинты на операции с автомобилями и пользователями, внутренняя аутентификация и через Google. Конфигурация для CORS, JWT, Swagger.
- Serilog
- Authentication Google
- Swashbuckle
- WebSocket
- SignalR

### Фоновая задача проверки и пуш-уведомления
`MaintenanceStatusChecker` проверяет через каждые 5 минут статус сервера в базе данных. Реализовано в двух экземплярах 
под WebSocket с приставкой `Ws` и для SignalR с `Sr`. Клиенты определенны на html с соответствующими приставками

### Логгирование
Логгирование происходит на уровне BLL и API. Запись в консоль и журнал в БД. Настройки в `appsettings.Development.json`

### Кастомные middleware
Абстрактный middleware `AbstractExceptionMiddleware`, который:
- Перехватывает все исключения,
- Логгирует исключения,
- Вызывает абстрактный метод `GetException` для получения кода состояния HTTP и сообщение об ошибке.

Класс `BusinessExceptionMiddleware`, расширяющий `AbstractExceptionMiddleware`, с реализацией метода `GetException`:
- Обрабатываются кастомные исключения и возвращаются соответствующие коды состояния
- Возвращается ответ, включающий сообщение об ошибке, тип исключения и детали ошибки

Класс `MaintenanceMiddleware` также проверяет статус тех. работ в БД.
Если сервер недоступен, не пропускает запрос и возвращает `503`. Определены разрешенные пути для просмотра уведомлений в 
списке `_excludedPaths`

### Кастомный action filter
Реализует метод до выполнения действия `OnActionExecuting`, который не пропускает запросы без Accept заголовка.
В `Program.cs` указан как глобальный фильтр для всех контроллеров.

### Контроллеры
- CRUD для автомобилей,
- CRUD для пользователей,
- Аутентификация и авторизация
	- Метод `SetTokenCookie` добавляет refresh токен в куки
- Регистрация и редактирование аккаунта

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/CarStockAPI/CarStockAPI/CarStockAPI.csproj", "CarStockAPI/"]
COPY ["/CarStockAPI/CarStockBLL/CarStockBLL.csproj", "CarStockBLL/"]
COPY ["/CarStockAPI/CarStockDAL/CarStockDAL.csproj", "CarStockDAL/"]
RUN dotnet restore "./CarStockAPI/CarStockAPI.csproj"
COPY . .
# Сборка
RUN dotnet build "CarStockAPI/CarStockAPI.csproj" -c Release --no-restore

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarStockAPI/CarStockAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY /CarStockAPI/CarStockAPI/appsettings.json /app/appsettings.json

# HTTPS конфигурация
RUN mkdir -p /app/Certificates
COPY /CarStockAPI/CarStockAPI/Certificates/aspnetapp.pfx /app/Certificates/
RUN chmod 644 /app/Certificates/aspnetapp.pfx

ENTRYPOINT ["dotnet", "CarStockAPIApp.dll"]
using CarStockDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CarStockAPI.Extensions
{
    /// <summary>
    /// Предоставляет метод-расширение для применения миграций базы данных при старте приложения
    /// </summary>
    public static class MigrationExtensions
    {
        /// <summary>
        /// Применяет ожидающие миграции базы данных с использованием провайдера сервисов приложения.
        /// </summary>
        /// <param name="app">Экземпляр построителя приложения.</param>
        public static void ApplyMigrations(this IApplicationBuilder app) 
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.SeedData();
            dbContext.Database.Migrate();
        }
    }
}

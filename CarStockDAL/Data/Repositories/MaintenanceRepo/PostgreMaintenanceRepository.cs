using CarStockDAL.Data.Interfaces.MaintenanceRepo;
using CarStockDAL.Models.WS;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories.WS
{
    /// <summary>
    /// Репозиторий для операций с тех. работами
    /// </summary>
    public class PostgreMaintenanceRepository : PostgreBaseRepository, IMaintenanceRepository
    {
        /// <summary>
        /// Коллекция сущностей тех. работ
        /// </summary>
        private readonly DbSet<Maintenance> _maintenances;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PostgreMaintenanceRepository"/>
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public PostgreMaintenanceRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _maintenances = dbContext.Maintenances;
        }

        /// <summary>
        /// Проверяет, активны ли тех. работы
        /// </summary>
        /// <returns> <c>True</c> Если активны, иначе <c>False</c></returns>
        public async Task<bool> IsMaintenanceActiveAsync()
        {
            var now = DateTime.UtcNow;
            return await _maintenances
                .AnyAsync(m => m.IsActive && m.StartTime <= now && m.EndTime >= now);
        }
       
        /// <summary>
        /// Создает запись о тех. работах
        /// </summary>
        /// <param name="start">Дата начала тех. работ</param>
        /// <param name="end">Дата окончание тех. работ</param>
        public async Task CreateMaintenanceAsync (DateTime start, DateTime end)
        {
            var maitenance = new Maintenance
            {
                StartTime = start,
                EndTime = end,
                IsActive = true
            };

            _maintenances.Add(maitenance);
            await SaveAsync();
        }

        /// <summary>
        /// Удаляет запись о тех. работах
        /// </summary>
        /// <param name="id">Идентификатор тех. работы</param>
        /// <returns></returns>
        public async Task DeleteMaintenanceByIdAsync(int id)
        {
            var maintenanceToDelete = await _maintenances.FindAsync(id);

            if (maintenanceToDelete != null)
            {
                _maintenances.Remove(maintenanceToDelete);
                await SaveAsync();
            }
        }
    }
}

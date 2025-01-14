namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Базовый репозиторий для работы с базой данных PostgreSQL
    /// </summary>
    public abstract class PostgreBaseRepository
    {
        /// <summary>
        /// Контекст базы данных для выполнения операций
        /// </summary>
        protected readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PostgreBaseRepository"/>.
        /// </summary>
        /// <param name="dbContext">Экземпляр контекста базы данных</param>
        protected PostgreBaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

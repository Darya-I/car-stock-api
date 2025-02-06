namespace CarStockDAL.Data.Interfaces.WS
{
    /// <summary>
    /// Интерфейс для операций над тех. работами в БД
    /// </summary>
    public interface IMaintenanceRepository
    {
        /// <summary>
        /// Проверяет, активны ли тех. работы
        /// </summary>
        /// <returns> <c>True</c> Если активны, иначе <c>False</c></returns>
        Task<bool> IsMaintenanceActiveAsync();

        /// <summary>
        /// Создает запись о тех. работах
        /// </summary>
        /// <param name="start">Дата начала тех. работ</param>
        /// <param name="end">Дата окончание тех. работ</param>
        Task CreateMaintenanceAsync(DateTime start, DateTime end);

        /// <summary>
        /// Удаляет запись о тех. работах
        /// </summary>
        /// <param name="id">Идентификатор тех. работы</param>
        /// <returns></returns>
        Task DeleteMaintenanceByIdAsync(int id);
    }
}
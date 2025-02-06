using CarStockBLL.DTO.Admin;

namespace CarStockBLL.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для операций с тех. работами
    /// </summary>
    public interface IMaitenanceService
    {
        /// <summary>
        /// Создает новую запись о тех. работах в базе данных
        /// </summary>
        Task CreateMaintenanceAsync();

        /// <summary>
        /// Удаляет запись о тех. работах из базы данных
        /// </summary>
        /// <param name="id"></param>
        Task DeleteMaintenanceByIdAsync(int id);
    }
}

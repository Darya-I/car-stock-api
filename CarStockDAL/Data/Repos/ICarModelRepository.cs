using CarStockDAL.Models;

namespace CarStockDAL.Data.Repos
{
    interface ICarModelRepository<T> : IDisposable
        where T : class
    {

        Task CreateCarModelAsync(T carModel);
        Task UpdateCarModelAsync(T carModel);
        Task DeleteCarModelAsync(int id);
        Task<T> GetCarModelByIdAsync(int id);
        Task<List<CarModel>> GetCarModelsAsync(bool tracked = true);
        Task SaveAsync();

    }
}

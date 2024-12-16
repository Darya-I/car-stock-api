using CarStockDAL.Models;

namespace CarStockDAL.Data.Repos
{
    interface IBrandRepository<T> : IDisposable
        where T : class
    {
        Task CreareBrandAsync(T brand);
        Task UpdateBrandAsync(T brand);
        Task DeleteBrandAsync(int id);
        Task<T> GetBrandByIdAsync(int id);
        Task<List<Brand>> GetAllBrandsAsync(bool tracked = true);
        Task SaveAsync();




    }
}

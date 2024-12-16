using CarStockDAL.Models;


namespace CarStockDAL.Data.Repos
{
    interface IColorRepository<T> : IDisposable
        where T : class
    {

        Task CreateColorAsync(T color);
        Task UpdateColorAsync(T color);
        Task DeleteColorAsync(int id);
        Task<T> GetColorByIdAsync(int id);
        Task<List<Color>> GetAllColorsAsync(bool tracked = true);
        Task SaveAsync();


    }
}

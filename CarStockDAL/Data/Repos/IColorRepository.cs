using CarStockDAL.Models;


namespace CarStockDAL.Data.Repos
{
    public interface IColorRepository<T>
        where T : class
    {

        Task CreateColorAsync(T color);
        Task UpdateColorAsync(T color);
        Task DeleteColorAsync(int id);
        Task<T> GetColorByIdAsync(int id);
        Task<List<Color>> GetAllColorsAsync(bool tracked = true);
        Task SaveAsync();

        Task<T> GetColorByNameAsync(string name);
    }
}

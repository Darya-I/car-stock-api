using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStockDAL.Data.Repositories
{
    /// <summary>
    /// Репозиторий для операций с пользователями, реализующий интерфейс <see cref="IUserRepository">
    /// </summary>
    public class PostgreUserRepository : IUserRepository
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория пользователей
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public PostgreUserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получает пользователя по refresh токену
        /// </summary>
        /// <param name="refreshToken">Refresh токен</param>
        /// <returns>Пользователь, связанный с указанным токеном обновления</returns>
        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return token;
        }

        /// <summary>
        /// Получает пользователя по имени пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>Пользователь или <c>null</c>, если пользователь не найден</returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        /// <summary>
        /// Получает пользователя по электронной почте
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        /// <returns>Пользователь или <c>null</c>, если пользователь не найден</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Обновляет refresh токен и срок его действия для указанного пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого обновляется токен</param>
        /// <param name="refrehToken">Новый refresh токен</param>
        /// <param name="refreshTokenExpireTime">Дата и время истечения срока действия токена</param>
        public async Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime)
        {
            user.RefreshToken = refrehToken;
            user.RefreshTokenExpireTime = refreshTokenExpireTime;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет информацию о пользователе
        /// </summary>
        /// <param name="user">Обновленный объект пользователя</param>
        public async Task UpdateUserAsync(User user)
        {
            _dbContext?.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

    }
}

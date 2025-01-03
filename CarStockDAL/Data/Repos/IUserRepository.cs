﻿using CarStockDAL.Models;

namespace CarStockDAL.Data.Repos
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(User user, string refrehToken, DateTime refreshTokenExpireTime);
        public Task UpdateUserAsync(User user);
    }
}

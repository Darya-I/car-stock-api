﻿namespace CarStockMAP.DTO
{
    public class LoginTokenDto
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
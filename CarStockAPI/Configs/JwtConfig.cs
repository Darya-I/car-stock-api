namespace CarStockAPI.Configs
{
    /// <summary>
    /// Конфигурация для настройки параметров JWT
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Секретный ключ для подписи
        /// </summary>
        public string Secret { get; set; }
        
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; set; }
        
        /// <summary>
        /// Аудитория токена
        /// </summary>
        public string Audience { get; set; }
    }
}
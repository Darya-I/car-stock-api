namespace CarStockAPI.Configs
{
    /// <summary>
    /// Конфигурация для настройки параметров аутентификации через Гугл
    /// </summary>
    public class GoogleConfig
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Секретный ключ пользователя
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
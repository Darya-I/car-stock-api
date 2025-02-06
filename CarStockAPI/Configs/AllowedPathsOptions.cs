namespace CarStockAPI.Configs
{
    /// <summary>
    /// Указание настроек для IOptions
    /// </summary>
    public class AllowedPathsOptions
    {
        /// <summary>
        /// Список допустимых путей, которые должен пропустить middleware проверки
        /// </summary>
        public List<string> Paths { get; set; } = new();
    }
}
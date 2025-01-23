namespace CarStockBLL.DTO
{
    /// <summary>
    /// DTO обновления количества автомобилей
    /// </summary>
    public class CarAmountDTO
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Количество автомобилей
        /// </summary>
        public int Amount { get; set; }
    }
}

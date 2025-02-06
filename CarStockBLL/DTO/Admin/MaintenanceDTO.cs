namespace CarStockBLL.DTO.Admin
{
    /// <summary>
    /// DTO для добавление тех. работ
    /// </summary>
    public class MaintenanceDTO
    {
        /// <summary>
        /// Дата и время начала тех. работ
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Дата и время окончания тех. работ
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}

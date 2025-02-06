namespace CarStockDAL.Models.WS
{
    public class Maintenance
    {
        /// <summary>
        /// Идентификатор тех. работы
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Дата и время начала тех. работ
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Дата и время конца тех. работ
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Флаг активности тех. работ
        /// </summary>
        public bool IsActive { get; set; }
    }
}

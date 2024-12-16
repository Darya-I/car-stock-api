namespace CarStockAPI.DTO
{
    public class CarInfoDTO
    {
        public string Description { get; set; }

        public CarInfoDTO MapToCarDTO(CarDTO carDTO) 
        {
           //       кастомный маппинг пока что
            //сюда еще можно другие строки запихать с мапами

            return new CarInfoDTO 
            {
                Description = $"{carDTO.Brand.Name}," +
                $" {carDTO.CarModel.Name}, {carDTO.CarModel.Colors}, " +
                $"количество на складе: {carDTO.Amount}," +
                $"{(carDTO.IsAvaible ? "доступен для заказа" : "не доступны для заказа")}"
            };
        }
    }
}

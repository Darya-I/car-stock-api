namespace CarStockAPI
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string CarModelName { get; set; }
        public string ColorName { get; set; }
        public int Amount { get; set; }
        public bool IsAvaible { get; set; }
    }
}

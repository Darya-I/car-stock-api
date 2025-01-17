/// <summary>
/// DTO для представления информации об автомобиле
/// </summary>
public class CarDTO
{
    /// <summary>
    /// Название бренда автомобиля
    /// </summary>
    public string BrandName { get; set; }

    /// <summary>
    /// Название модели автомобиля
    /// </summary>
    public string CarModelName { get; set; }

    /// <summary>
    /// Название цвета автомобиля
    /// </summary>
    public string ColorName { get; set; }

    /// <summary>
    /// Количество автомобилей в наличии
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Доступность автомобиля для покупки
    /// </summary>
    public bool IsAvailable { get; set; }
}

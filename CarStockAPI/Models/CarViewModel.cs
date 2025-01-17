/// <summary>
/// Модель отображения данных автомобиля для клиента
/// </summary>
public class CarViewModel
{
    /// <summary>
    /// Уникальный идентификатор автомобиля
    /// </summary>
    public int Id { get; set; }

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
    /// Количество доступных автомобилей
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Доступность автомобиля
    /// </summary>
    public bool IsAvaible { get; set; }
}

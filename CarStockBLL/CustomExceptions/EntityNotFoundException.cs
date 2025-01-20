namespace CarStockBLL.CustomException
{
    /// <summary>
    /// Исключение, которое возникает, когда объект не найден
    /// Наследуется от <see cref="ApiException"/>.
    /// </summary>
    public class EntityNotFoundException : ApiException
    {
        //                                  Дефолтные конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityNotFoundException"/>.
        /// </summary>
        public EntityNotFoundException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityNotFoundException"/> с указанным сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        public EntityNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityNotFoundException"/> с указанным сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        /// <param name="inner">Исключение, вызвавшее текущее исключение</param>
        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
namespace CarStockBLL.CustomException
{
    /// <summary>
    /// Кастомное глобальное исключение для обработки ошибок из бизнес слоя
    /// Наследуется от <see cref="Exception"/>.   
    /// </summary>
    public class ApiException : Exception
    {
        //                                  Дефолтные конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiException"/>.
        /// </summary>
        public ApiException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiException"/> с указанным сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        public ApiException(string message)
            : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApiException"/> с указанным сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        /// <param name="inner">Исключение, вызвавшее текущее исключение</param>
        public ApiException(string message, Exception inner)
            : base(message, inner) { }
    }
}
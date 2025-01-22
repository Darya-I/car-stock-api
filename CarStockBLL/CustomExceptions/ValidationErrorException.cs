namespace CarStockBLL.CustomException
{
    /// <summary>
    /// Исключение при ошибках валидации данных
    /// Наследуется от <see cref="ApiException"/>.
    /// </summary>
    public class ValidationErrorException : ApiException
    {
        //                                  Дефолтные конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValidationErrorException"/>.
        /// </summary>
        public ValidationErrorException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValidationErrorException"/> с указанным сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        public ValidationErrorException(string message)
            : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValidationErrorException"/> с указанным сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        /// <param name="inner">Исключение, вызвавшее текущее исключение</param>
        public ValidationErrorException(string message, Exception inner)
            : base(message, inner) { }
    }
}

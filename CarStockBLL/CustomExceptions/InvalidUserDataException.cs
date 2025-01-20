namespace CarStockBLL.CustomException
{
    /// <summary>
    /// Исключение при ошибках валидации данных от пользователя
    /// Наследуется от <see cref="ApiException"/>.
    /// </summary>
    public class InvalidUserDataException : ApiException
    {
        //                                  Дефолтные конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidUserDataException"/>.
        /// </summary>
        public InvalidUserDataException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidUserDataException"/> с указанным сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        public InvalidUserDataException(string message)
            : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidUserDataException"/> с указанным сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        /// <param name="inner">Исключение, вызвавшее текущее исключение</param>
        public InvalidUserDataException(string message, Exception inner)
            : base(message, inner) { }
    }
}
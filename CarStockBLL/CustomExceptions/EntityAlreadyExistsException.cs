namespace CarStockBLL.CustomException
{
    /// <summary>
    /// Исключение, которое возникает, когда объект уже был создан
    /// Наследуется от <see cref="ApiException"/>.
    /// </summary>
    public class EntityAlreadyExistsException : ApiException
    {
        //                                  Дефолтные конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityAlreadyExistsException"/>.
        /// </summary>
        public EntityAlreadyExistsException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityAlreadyExistsException"/> с указанным сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        public EntityAlreadyExistsException(string message)
            : base(message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityAlreadyExistsException"/> с указанным сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку</param>
        /// <param name="inner">Исключение, вызвавшее текущее исключение</param>
        public EntityAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
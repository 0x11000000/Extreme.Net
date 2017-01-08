using System;

namespace Extreme.Net
{
    /// <summary>
    /// Исключение, которое выбрасывается, в случае возникновения ошибки при работе с HTTP-протоколом.
    /// </summary>
    internal sealed class HttpException : NetException
    {
        #region Свойства (открытые)

        /// <summary>
        /// Возвращает состояние исключения.
        /// </summary>
        public HttpExceptionStatus Status { get; internal set; }

        /// <summary>
        /// Возвращает код состояния ответа от HTTP-сервера.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; private set; }

        #endregion


        internal bool EmptyMessageBody { get; set; }


        #region Конструкторы (открытые)

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HttpException"/>.
        /// </summary>
        public HttpException() : this(Resources.HttpException_Default) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HttpException"/> заданным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причины исключения.</param>
        /// <param name="innerException">Исключение, вызвавшее текущие исключение, или значение <see langword="null"/>.</param>
        public HttpException(string message, Exception innerException = null)
            : base(message, innerException) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HttpException"/> заданным сообщением об ошибке и кодом состояния ответа.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причины исключения.</param>
        /// <param name="statusCode">Код состояния ответа от HTTP-сервера.</param>
        /// <param name="innerException">Исключение, вызвавшее текущие исключение, или значение <see langword="null"/>.</param>
        public HttpException(string message, HttpExceptionStatus status,
            HttpStatusCode httpStatusCode = HttpStatusCode.None, Exception innerException = null)
            : base(message, innerException)
        {
            Status = status;
            HttpStatusCode = httpStatusCode;
        }

        #endregion

    }
}

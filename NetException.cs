using System;

namespace Extreme.Net
{
    /// <summary>
    /// Исключение, которое выбрасывается, в случае возникновения ошибки при работе с сетью.
    /// </summary>
    public class NetException : Exception
    {
        #region Конструкторы (открытые)

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NetException"/>.
        /// </summary>
        public NetException() : this(Resources.NetException_Default) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NetException"/> заданным сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке с объяснением причины исключения.</param>
        /// <param name="innerException">Исключение, вызвавшее текущие исключение, или значение <see langword="null"/>.</param>
        public NetException(string message, Exception innerException = null)
            : base(message, innerException) { }

        #endregion

    }
}

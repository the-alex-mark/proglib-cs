using System;

namespace ProgLib.Network.Tcp
{
    public class TcpErrorEventArgs : EventArgs
    {
        public TcpErrorEventArgs(Exception Exception)
        {
            this.Exception = Exception;
        }

        /// <summary>
        /// Ошибка при работе сервера
        /// </summary>
        public Exception Exception { get; }
    }
}

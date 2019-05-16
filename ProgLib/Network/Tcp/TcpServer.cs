using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using ProgLib.IO;

namespace ProgLib.Network.Tcp
{
    /// <summary>
    /// Предоставляет методы для работы с сервером протокола TCP/IP
    /// </summary>
    public class TcpServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Port">Номер порта, связанный с адресом, или любой доступный порт</param>
        /// <param name="Backlog">Максимальное количество возможных подключений</param>
        public TcpServer(Int32 Port, Int32 Backlog)
        {
            // Инициализация сервера
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, Port));
            _server.Listen(Backlog);

            // Получение настроек сервера
            _address = IPAddress.Any;
            _port = Port;
            _backlog = Backlog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">IP</param>
        /// <param name="Port">Номер порта, связанный с адресом, или любой доступный порт</param>
        /// <param name="Backlog">Максимальное количество возможных подключений</param>
        public TcpServer(IPAddress Address, Int32 Port, Int32 Backlog)
        {
            // Инициализация сервера
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(Address, Port));
            _server.Listen(Backlog);

            // Получение настроек сервера
            _address = Address;
            _port = Port;
            _backlog = Backlog;
        }

        #region Variables

        // Настройки сервера
        private Socket _server;
        private Int32 _port;
        private IPAddress _address;
        private Int32 _backlog;
        private String _data = null;

        // Внешний поток
        private Thread _flow;

        /// <summary>
        /// Событие при получении данных от клиента.
        /// </summary>
        public event TcpReceiverEventHandler Receiver;
        public delegate void TcpReceiverEventHandler(Object sender, TcpReceiverEventArgs eventArgs);

        /// <summary>
        /// Событие при возникновении ошибок в работе сервера.
        /// </summary>
        public event TcpErrorEventHandler Error;
        public delegate void TcpErrorEventHandler(Object sender, TcpErrorEventArgs eventArgs);

        #endregion

        #region Method

        /// <summary>
        /// Получает имя клиента из указанного сокета.
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static String GetHostName(Socket Client)
        {
            String IP = ((IPEndPoint)Client.RemoteEndPoint).Address.ToString();
            return Dns.GetHostEntry(IP).HostName;
        }

        /// <summary>
        /// Преобразует строку в массив байтов.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        [Obsolete("")]
        public static Byte[] GetBytes(String Value)
        {
            return Encoding.UTF8.GetBytes(Value);
        }

        /// <summary>
        /// Преобразует массив байт в строчное представление.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        [Obsolete("")]
        public static String GetString(Byte[] Value)
        {
            return Encoding.UTF8.GetString(
                Value.TakeWhile((v, index) => Value.Skip(index).Any(w => w != 0x00)).ToArray());
        }

        /// <summary>
        /// Преобразует массив байт в строчное представление.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        [Obsolete("")]
        public static String GetString(Byte[] Value, Int32 Count)
        {
            return Encoding.UTF8.GetString(Value, 0, Count);
        }

        #endregion

        /// <summary>
        /// Запускает процесс получения данных.
        /// </summary>
        public void Start()
        {
            Stop();

            _flow = new Thread(new ThreadStart(Listener));
            //_flow.SetApartmentState(ApartmentState.STA);
            _flow.IsBackground = true;
            _flow.Start();
        }

        /// <summary>
        /// Запускает процесс получения и трансляции данных.
        /// </summary>
        /// <param name="Data">Транслируемые данные</param>
        public void Start(String Data)
        {
            _data = Data;
            Start();
        }

        /// <summary>
        /// Метод получения данных от клиента.
        /// </summary>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityException"></exception>
        private void Listener()
        {
            while (true)
            {
                try
                {
                    // Получение клиентского сокета
                    Socket _client = _server.Accept();

                    // Получение входящих данных
                    BytesBuilder _bytes = new BytesBuilder();
                    do
                    {
                        Byte[] _buffer = new Byte[1024];
                        Int32 _length = _client.Receive(_buffer);

                        _bytes.Append(_buffer, _length, false);
                    }
                    while (_client.Available > 0);

                    Byte[] __data = _bytes.ToArray();
                    _bytes.Dispose();

                    // Обработка события при получении данных
                    Receiver?.Invoke(this, new TcpReceiverEventArgs(_client, __data, __data.Length));

                    if (_data != "" && _data != null)
                    {
                        // Отправка данных клиенту
                        _client.Send(TcpServer.GetBytes(_data));
                    }

                    // Закрытие клиентского сокета
                    _client.Shutdown(SocketShutdown.Both);
                    _client.Close();
                }
                catch (Exception Exception)
                {
                    // Обработка события при возникновении ошибок в работе сервера
                    Error?.Invoke(this, new TcpErrorEventArgs(Exception));
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Boolean Send(String Server, Byte[] Buffer)
        {
            return Send(Server, _port, Buffer);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Boolean Send(String Server, Int32 Port, Byte[] Buffer)
        {
            try
            {
                // Инициализация клиентского сокета
                Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client.Connect(Server, Port);

                // Отправление данных на сервер
                _client.Send(Buffer);

                // Закрытие клиентского сокета
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();

                return true;
            }
            catch (Exception Exception)
            {
                // Обработка события при возникновении ошибок в работе сервера
                Error?.Invoke(this, new TcpErrorEventArgs(Exception));

                return false;
            }
        }

        /// <summary>
        /// Завершает процесс получения данных.
        /// </summary>
        /// <exception cref="SecurityException"></exception>
        public void Stop()
        {
            if (_flow != null)
                _flow.Interrupt();

            _flow = null;
            _data = null;
        }

        /// <summary>
        /// Завершает работу сервера и освобождает все связанные ресурсы.
        /// </summary>
        public void Close()
        {
            Stop();
            _server.Close();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="TcpServer"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
            _server.Dispose();
        }
    }
}

using System;
using System.Data;
using System.Data.MySqlClient;

namespace ProgLib.Data.MySql
{
    /// <summary>
    /// Предоставляет методы для работы с базой данных MySql.
    /// </summary>
    public class MySqlDataBase
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public MySqlDataBase(MySqlConnectionStringBuilder ConnectionString)
        {
            Connection = new MySqlConnection(ConnectionString.ToString());
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public MySqlDataBase(String ConnectionString)
        {
            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="Server">Имя сервера</param>
        /// <param name="DataBase">Имя базы данных</param>
        /// <param name="User">Пользователь</param>
        /// <param name="Password">Пароль</param>
        public MySqlDataBase(String Server, String DataBase, String User, String Password)
        {
            Connection = new MySqlConnection($"Server={Server};Database={DataBase};User Id={User};Password={Password}");
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="Server">Имя сервера</param>
        /// <param name="Port">Номер порта</param>
        /// <param name="DataBase">Имя базы данных</param>
        /// <param name="User">Пользователь</param>
        /// <param name="Password">Пароль</param>
        public MySqlDataBase(String Server, Int32 Port, String DataBase, String User, String Password)
        {
            Connection = new MySqlConnection($"Server={Server};Port={Port};Database={DataBase};User Id={User};Password={Password}");
            Connection.Open();
        }
        
        #region Variables

        private MySqlConnection Connection;

        #endregion
        
        /// <summary>
        /// Получает состояние подключения базы данных.
        /// </summary>
        /// <returns></returns>
        public ConnectionState State()
        {
            return Connection.State;
        }

        /// <summary>
        /// Получает список таблиц текущей базы данных.
        /// </summary>
        /// <returns></returns>
        public MySqlResult ShowTables()
        {
            return this.Request("Show Tables;");
        }

        /// <summary>
        /// Получает структуру таблицы.
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public MySqlResult Describe(String Table)
        {
            return this.Request($"Describe {Table};");
        }

        /// <summary>
        /// Обрабатывает запрос и возвращает полученные данные.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public MySqlResult Request(String Command)
        {
            try
            {
                MySqlCommand Request = new MySqlCommand(Command, Connection);

                MySqlDataAdapter Adapter = new MySqlDataAdapter(Request);
                DataTable Result = new DataTable();
                Adapter.Fill(Result);

                return new MySqlResult(Result, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new MySqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Закрывает подключение к базе данных.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="MySqlDataBase"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
            Connection.Dispose();
        }
    }
}

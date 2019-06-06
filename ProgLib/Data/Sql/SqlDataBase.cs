using System;
using System.Data;
using System.Data.SqlClient;

namespace ProgLib.Data.Sql
{
    /// <summary>
    /// Предоставляет методы для работы с базой данных MSSql.
    /// </summary>
    public class SqlDataBase
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public SqlDataBase(SqlConnectionStringBuilder ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString.ToString());
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public SqlDataBase(String ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="Server">Имя сервера</param>
        /// <param name="DataBase">Имя базы данных</param>
        /// <param name="User">Пользователь</param>
        /// <param name="Password">Пароль</param>
        public SqlDataBase(String Server, String DataBase = "master", String User = "", String Password = "")
        {
            String ConnectionString
                = (User != "" && Password != "")
                ? String.Format(@"Data Source={0};Initial Catalog={1};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;User ID={2};Password={3}", Server, DataBase, User, Password)
                : String.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", Server, DataBase);
            
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        #region Variables

        private SqlConnection Connection;

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
        public SqlResult ShowTables()
        {
            return this.Request("Select Name from sys.objects Where Type IN (N'U') and Name <> 'sysdiagrams'");
        }

        /// <summary>
        /// Получает структуру таблицы.
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public SqlResult Describe(String Table)
        {
            return this.Request("Select * from INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = N'" + Table + "'");
        }

        /// <summary>
        /// Обрабатывает запрос и возвращает полученные данные.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public SqlResult Request(String Command)
        {
            try
            {
                SqlCommand Request = new SqlCommand(Command, Connection);

                SqlDataAdapter Adapter = new SqlDataAdapter(Request);
                DataTable Result = new DataTable();
                Adapter.Fill(Result);

                return new SqlResult(Result, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new SqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Закрывает подключение к базе данных.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="SqlDataBase"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
            Connection.Dispose();
        }
    }
}

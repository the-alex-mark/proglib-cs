using System;
using System.Data;
using System.Data.SqlClient;

namespace ProgLib.Data.Sql
{
    /// <summary>
    /// Предоставляет методы для работы с сервером MSSql.
    /// </summary>
    public class SqlServer
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlServer"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public SqlServer(SqlConnectionStringBuilder ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString.ToString());
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlServer"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString"></param>
        public SqlServer(String ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlServer"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="User"></param>
        /// <param name="Password"></param>
        public SqlServer(String Server, String User = "", String Password = "")
        {
            String ConnectionString
                = (User != "" && Password != "")
                ? String.Format(@"Data Source={0};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;User ID={1};Password={2}", Server, User, Password)
                : String.Format(@"Data Source={0};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", Server);

            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        #region Variables

        private SqlConnection Connection;

        #endregion

        /// <summary>
        /// Присоединяет базу данных.
        /// </summary>
        /// <param name="Name">Новое имя базы данных</param>
        /// <param name="DataBase">Расположение подключаемой базы данных</param>
        /// <param name="Log">Расположение Log файла подключаемой базы данных</param>
        /// <returns></returns>
        public SqlResult ConnectDataBase(String Name, String DataBase, String Log)
        {
            return this.Request(
                " Use master;" +
                " Create DataBase " + Name + " on" +
                " (FILENAME = '" + DataBase + "'), (FILENAME = '" + Log + "')" +
                " For Attach; ");
        }

        /// <summary>
        /// Отсоединяет базу данных.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public SqlResult DisconnectDataBase(String Name)
        {
            return this.Request(
                " Use master;" +
                " Alter DataBase [" + Name + "] Set SINGLE_USER With Rollback Immediate;" +
                " Exec sp_detach_db @dbname = N'" + Name + "';");
        }

        /// <summary>
        /// Создаёт новую базу данных.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public SqlResult CreateDataBase(String Name)
        {
            return this.Request("Create DataBase " + Name);
        }

        /// <summary>
        /// Удаляет указанную базу данных.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public SqlResult DropDataBase(String Name)
        {
            return this.Request("Drop DataBase " + Name);
        }
        
        /// <summary>
        /// Получает список баз данных.
        /// </summary>
        /// <returns></returns>
        public SqlResult ShowDataBases()
        {
            return this.Request("Select Name from sys.databases");
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
        /// Закрывает подключение к серверу.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="SqlServer"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
            Connection.Dispose();
        }
    }
}

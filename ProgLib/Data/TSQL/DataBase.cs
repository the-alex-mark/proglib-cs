using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProgLib.Data.TSQL
{
    public class DataBase
    {
        public DataBase(String ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        public DataBase(String Server, String DataBase = "master", String User = "", String Password = "")
        {
            String ConnectionString
                = (User != "" && Password != "")
                ? String.Format(@"Data Source={0};Initial Catalog={1};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;User ID={2};Password={3}", Server, DataBase, User, Password)
                : String.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", Server, DataBase);
            
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }
        
        private SqlConnection Connection;

        /// <summary>
        /// Получает состояние подключения базы данных
        /// </summary>
        /// <returns></returns>
        public ConnectionState State()
        {
            return Connection.State;
        }
        
        public void Close()
        {
            Connection.Close();
        }
        public void Dispose()
        {
            Connection.Dispose();
        }

        /// <summary>
        /// Получает список таблиц текущей базы данных
        /// </summary>
        /// <returns></returns>
        public SqlResult ShowTables()
        {
            try
            {
                SqlCommand Select = new SqlCommand(
                    "Select Name from sys.objects Where Type IN (N'U') and Name <> 'sysdiagrams'", Connection);

                SqlDataAdapter Adapter = new SqlDataAdapter(Select);
                DataTable Result = new DataTable();
                Adapter.Fill(Result);

                return new SqlResult(Result, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new SqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Получает структуру таблицы
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public SqlResult Describe(String Table)
        {
            try
            {
                SqlCommand Request = new SqlCommand(
                    "Select * from INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = N'" + Table + "'", Connection);

                SqlDataAdapter Adapter = new SqlDataAdapter(Request);
                DataTable Result = new DataTable();
                Adapter.Fill(Result);

                return new SqlResult(Result, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new SqlResult(new DataTable(), _exception.Message); }
        }
        
        /// <summary>
        /// Обрабатывает запрос
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
    }
}

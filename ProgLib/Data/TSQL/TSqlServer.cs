using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Data.TSQL
{
    public class TSqlServer
    {
        public TSqlServer(String ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        public TSqlServer(String Server, String User = "", String Password = "")
        {
            String ConnectionString
                = (User != "" && Password != "")
                ? String.Format(@"Data Source={0};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;User ID={1};Password={2}", Server, User, Password)
                : String.Format(@"Data Source={0};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", Server);

            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        private SqlConnection Connection;

        /// <summary>
        /// Присоединяет базу данных
        /// </summary>
        /// <param name="DataBase">Расположение подключаемой базы данных</param>
        /// <param name="Log">Расположение Log файла подключаемой базы данных</param>
        /// <returns></returns>
        public TSqlResult ConnectDataBase(String DataBase, String Log)
        {
            try
            {
                String Command
                    = " USE master;"
                    + " Create DataBase UchProc_Круглов on"
                    + " (FILENAME = '" + DataBase + "'), (FILENAME = '" + Log + "')"
                    + " For Attach; ";

                SqlCommand Create = new SqlCommand(Command, Connection);
                Create.ExecuteNonQuery();

                return new TSqlResult(new DataTable(), "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new TSqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Отсоединяет базу данных
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public TSqlResult DisconnectDataBase(String Name)
        {
            try
            {
                String Command
                    = " Use master;"
                    + " Alter DataBase [" + Name + "] Set SINGLE_USER With Rollback Immediate;"
                    + " Exec sp_detach_db @dbname = N'" + Name + "';";

                SqlCommand Create = new SqlCommand(Command, Connection);
                Create.ExecuteNonQuery();

                return new TSqlResult(new DataTable(), "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new TSqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Создаёт новую базу данных
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public TSqlResult CreateDataBase(String Name)
        {
            try
            {
                SqlCommand Create = new SqlCommand("Create DataBase " + Name, Connection);
                Create.ExecuteNonQuery();

                return new TSqlResult(new DataTable(), "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new TSqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Удаляет указанную базу данных
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public TSqlResult DropDataBase(String Name)
        {
            try
            {
                SqlCommand Create = new SqlCommand("Drop DataBase " + Name, Connection);
                Create.ExecuteNonQuery();
                
                return new TSqlResult(new DataTable(), "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new TSqlResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Получает список баз данных
        /// </summary>
        /// <returns></returns>
        public TSqlResult ShowDataBases()
        {
            try
            {
                SqlCommand Select = new SqlCommand("Select Name from sys.databases", Connection);

                SqlDataAdapter Adapter = new SqlDataAdapter(Select);
                DataTable DT = new DataTable();
                Adapter.Fill(DT);

                return new TSqlResult(DT, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new TSqlResult(new DataTable(), _exception.Message); }
        }
    }
}

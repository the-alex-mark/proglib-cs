using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Data.MySql
{
    /// <summary>
    /// Предоставляет методы для работы с базой данных Access.
    /// </summary>
    public class MySqlDataBase
    {
        public MySqlDataBase(String ConnectionString)
        {
            Connection = new MySqlConnection(ConnectionString);
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
            return this.Request("Select MSysObjects.Name from MSysObjects " +
                                "Where Left([Name], 1) <> '~' and Left([Name], 4) <> 'MSys' and MSysObjects.Type In(1, 4, 6) and [Name] <> 'f_9E8203D96A754B0890DAF9414007C362_Data'");
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

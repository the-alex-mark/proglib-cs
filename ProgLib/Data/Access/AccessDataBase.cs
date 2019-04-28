using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Data.Access
{
    /// <summary>
    /// Предоставляет методы для работы с базой данных Access.
    /// </summary>
    public class AccessDataBase
    {
        public AccessDataBase(String DataBase)
        {
            Connection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={DataBase};");
            Connection.Open();
        }

        public AccessDataBase(String DataBase, String Password)
        {
            Connection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={DataBase};Jet OLEDB:Database Password={Password};");
            Connection.Open();
        }

        public AccessDataBase(String DataBase, String SystemDataBase, String User, String Password)
        {
            Connection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={DataBase};Jet OLEDB:System Database={SystemDataBase};User ID={User};Password={Password};");
            Connection.Open();
        }

        #region Global Variables

        private OleDbConnection Connection;

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
        public AccessResult ShowTables()
        {
            return this.Request("Select MSysObjects.Name from MSysObjects " +
                                "Where Left([Name], 1) <> '~' and Left([Name], 4) <> 'MSys' and MSysObjects.Type In(1, 4, 6) and [Name] <> 'f_9E8203D96A754B0890DAF9414007C362_Data'");
        }

        /// <summary>
        /// Обрабатывает запрос и возвращает полученные данные.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public AccessResult Request(String Command)
        {
            try
            {
                OleDbCommand Request = new OleDbCommand(Command, Connection);

                OleDbDataAdapter Adapter = new OleDbDataAdapter(Request);
                DataTable Result = new DataTable();
                Adapter.Fill(Result);

                return new AccessResult(Result, "Command(s) completed successfully.");
            }
            catch (Exception _exception) { return new AccessResult(new DataTable(), _exception.Message); }
        }

        /// <summary>
        /// Закрывает подключение к базе данных.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="AccessDataBase"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
            Connection.Dispose();
        }
    }
}

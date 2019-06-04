using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="ConnectionString">Строка подключения</param>
        public AccessDataBase(String ConnectionString)
        {
            Connection = new OleDbConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="DataBase">Расположения базы данных</param>
        public AccessDataBase(FileInfo DataBase)
        {
            Connection = new OleDbConnection($"Provider={GetProvider(DataBase.Extension)};Data Source={DataBase.FullName};");
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="DataBase">Расположение базы данных</param>
        /// <param name="Password">Пароль</param>
        public AccessDataBase(FileInfo DataBase, String Password)
        {
            Connection = new OleDbConnection($"Provider={GetProvider(DataBase.Extension)};Data Source={DataBase.FullName};Jet OLEDB:Database Password={Password};");
            Connection.Open();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessDataBase"/> и осуществляет подключение по указанным данным.
        /// </summary>
        /// <param name="DataBase">Имя базы данных</param>
        /// <param name="SystemDataBase">Имя системной базы данных (для работы некоторых функций)</param>
        /// <param name="User">Пользователь</param>
        /// <param name="Password">Пароль</param>
        public AccessDataBase(FileInfo DataBase, FileInfo SystemDataBase, String User, String Password)
        {
            Connection = new OleDbConnection($"Provider={GetProvider(DataBase.Extension)};Data Source={DataBase.FullName};Jet OLEDB:System Database={SystemDataBase.FullName};User ID={User};Password={Password};");
            Connection.Open();
        }

        #region Variables

        private OleDbConnection Connection;

        #endregion

        #region Methods

        /// <summary>
        /// Получает провайдер для подключения к базе данных исходя из расширения её файла.
        /// </summary>
        /// <param name="Extension">Расширение файла базы данных</param>
        /// <returns></returns>
        private String GetProvider(String Extension)
        {
            return (Extension == ".accdb") ? "Microsoft.ACE.OLEDB.12.0" : "Microsoft.Jet.OLEDB.4.0";
        }

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

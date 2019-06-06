using System;
using System.Data;

namespace ProgLib.Data.OleDb
{
    /// <summary>
    /// Представляет результат при выполении запроса к базе данных Access.
    /// </summary>
    public class OleDbResult
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OleDbResult"/>.
        /// </summary>
        /// <param name="Message"></param>
        public OleDbResult(String Message)
        {
            this.Table = new DataTable();
            this.Message = Message;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OleDbResult"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Message"></param>
        public OleDbResult(DataTable Table, String Message)
        {
            this.Table = Table;
            this.Message = Message;
        }

        #region Properties

        /// <summary>
        /// Результат выполнения запроса на выборку
        /// </summary>
        public DataTable Table
        {
            get;
        }

        /// <summary>
        /// Статус выволнения запроса
        /// </summary>
        public String Message
        {
            get;
        }

        #endregion
    }
}
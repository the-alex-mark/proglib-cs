using System;
using System.Data;

namespace ProgLib.Data.Sql
{
    /// <summary>
    /// Представляет результат при выполении запроса к базе данных MSSql.
    /// </summary>
    public class SqlResult
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlResult"/>.
        /// </summary>
        /// <param name="Message"></param>
        public SqlResult(String Message)
        {
            this.Table = new DataTable();
            this.Message = Message;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlResult"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Message"></param>
        public SqlResult(DataTable Table, String Message)
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

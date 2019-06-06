using System;
using System.Data;

namespace ProgLib.Data.MySql
{
    /// <summary>
    /// Представляет результат при выполении запроса к базе данных MySql.
    /// </summary>
    public class MySqlResult
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlResult"/>.
        /// </summary>
        /// <param name="Message"></param>
        public MySqlResult(String Message)
        {
            this.Table = new DataTable();
            this.Message = Message;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MySqlResult"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Message"></param>
        public MySqlResult(DataTable Table, String Message)
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

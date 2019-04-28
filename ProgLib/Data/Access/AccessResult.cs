using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Data.Access
{
    public class AccessResult
    {
        public AccessResult(DataTable Table, String Status)
        {
            this.Table = Table;
            this.Status = Status;
        }
        
        /// <summary>
        /// Возвращает результат выполнения запроса на выборку
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// Возвращает статус выволнения запроса
        /// </summary>
        public String Status { get; set; }
    }
}

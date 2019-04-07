using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio
{
    /// <summary>
    /// Класс для работы с интернет радиостанциями
    /// </summary>
    public class Radio : Record
    {
        /// <summary>
        /// Объявляет экземпляр для работы с интернет радиостанцией
        /// </summary>
        /// <param name="File"></param>
        public Radio(String URL)
        {
            if (URL.ToLower().StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || URL.ToLower().StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
            {
                this.Name = "";
                this.URL = URL;
            }
            else { throw new Exception("Данный URL-Адрес не является адресом интернет радиостанции."); }
        }

        /// <summary>
        /// Объявляет экземпляр для работы с интернет радиостанцией
        /// </summary>
        /// <param name="File"></param>
        public Radio(String Name, String URL)
        {
            if (URL.ToLower().StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || URL.ToLower().StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
            {
                this.Name = Name;
                this.URL = URL;
            }
            else { throw new Exception("Данный URL-Адрес не является адресом интернет радиостанции."); }
        }

        /// <summary>
        /// Название радиостации
        /// </summary>
        public String Name { get; }
    }
}

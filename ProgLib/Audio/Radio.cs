using System;

namespace ProgLib.Audio
{
    /// <summary>
    /// Предоставляет методы для работы с интернет радиостанцией
    /// </summary>
    public class Radio : Record
    {
        /// <summary>
        /// Инициализирует экземпляр типа <see cref="Radio"/> для работы с интернет радиостанцией.
        /// </summary>
        /// <param name="URL"></param>
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
        /// Инициализирует экземпляр типа <see cref="Radio"/> для работы с интернет радиостанцией.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="URL"></param>
        public Radio(String Name, String URL)
        {
            if (URL.IsRadio())
            {
                this.Name = Name;
                this.URL = URL;
            }
            else { throw new Exception("Данная строка не является адресом интернет радиостанции."); }
        }

        #region Properties

        /// <summary>
        /// Название радиостации
        /// </summary>
        public String Name
        {
            get;
        }

        #endregion
    }
}

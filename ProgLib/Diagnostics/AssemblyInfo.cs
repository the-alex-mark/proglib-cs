using System;
using System.Diagnostics;
using ProgLib.Data.CSharp;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Класс для работы с метаданными
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// Получает сведения о сборке
        /// </summary>
        /// <param name="Assembly">Расположение приложения</param>
        public static Property Get(String Assembly)
        {
            FileVersionInfo _information = FileVersionInfo.GetVersionInfo(Assembly);
            Property _property = new Property
            {
                Title = _information.ProductName,
                Description = _information.FileDescription,
                Configuration = "",
                Company = _information.CompanyName,
                Product = _information.ProductName,
                Copyright = _information.LegalCopyright,
                Trademark = _information.LegalTrademarks,
                Culture = "",
                ComVisible = false,
                Version = System.Reflection.Assembly.LoadFile(Assembly).GetName().Version,
                FileVersion = new Version(_information.FileVersion),
            };

            return _property;
        }
    }
}
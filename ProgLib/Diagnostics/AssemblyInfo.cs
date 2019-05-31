using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using ProgLib.Data;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет методы для работы с метаданными исполняемого файла.
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// Инициализирует экземпляр типа <see cref="AssemblyInfo"/>.
        /// </summary>
        public AssemblyInfo()
        {
            this.Title = "";
            this.Description = "";
            this.Configuration = "";
            this.Company = "";
            this.Product = "";
            this.Copyright = "Copyright ©  " + DateTime.Now.Year.ToString();
            this.Trademark = "";
            this.Culture = "";

            this.ComVisible = false;

            this.Version = new Version(1, 0, 0, 0);
            this.FileVersion = new Version(1, 0, 0, 0);
        }

        /// <summary>
        /// Инициализирует экземпляр типа <see cref="AssemblyInfo"/>.
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="Configuration"></param>
        /// <param name="Company"></param>
        /// <param name="Product"></param>
        /// <param name="Copyright"></param>
        /// <param name="Trademark"></param>
        /// <param name="Culture"></param>
        /// <param name="ComVisible"></param>
        /// <param name="Guid"></param>
        /// <param name="Version"></param>
        /// <param name="FileVersion"></param>
        public AssemblyInfo(String Title, String Description, String Configuration, String Company, String Product, String Copyright, String Trademark, String Culture, Boolean ComVisible, String Guid, Version Version, Version FileVersion)
        {
            this.Title = Title;
            this.Description = Description;
            this.Configuration = Configuration;
            this.Company = Company;
            this.Product = Product;
            this.Copyright = Copyright;
            this.Trademark = Trademark;
            this.Culture = Culture;

            this.ComVisible = ComVisible;

            this.Version = Version;
            this.FileVersion = FileVersion;
        }

        #region Properties

        /// <summary>
        /// Название
        /// </summary>
        public String Title
        {
            get;
            set;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Конфигурация
        /// </summary>
        public String Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Компания
        /// </summary>
        public String Company
        {
            get;
            set;
        }

        /// <summary>
        /// Название продукта
        /// </summary>
        public String Product
        {
            get;
            set;
        }

        /// <summary>
        /// Авторские права
        /// </summary>
        public String Copyright
        {
            get;
            set;
        }

        /// <summary>
        /// Товарный знак
        /// </summary>
        public String Trademark
        {
            get;
            set;
        }

        /// <summary>
        /// Культура
        /// </summary>
        public String Culture
        {
            get;
            set;
        }

        /// <summary>
        /// Видимость типов данной сборки для компонентов COM.
        /// </summary>
        public Boolean ComVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Сведения о версии сборки.
        /// </summary>
        public Version Version
        {
            get;
            set;
        }

        /// <summary>
        /// Сведения о версии файла.
        /// </summary>
        public Version FileVersion
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Получает сведения о сборке.
        /// </summary>
        /// <param name="File">Расположение исполняющего файла</param>
        /// <returns></returns>
        public static AssemblyInfo Load(String File)
        {
            FileVersionInfo _fileInfo = FileVersionInfo.GetVersionInfo(File);
            AssemblyInfo _assemblyInfo = new AssemblyInfo
            {
                Title = _fileInfo.ProductName,
                Description = _fileInfo.FileDescription,
                Configuration = "",
                Company = _fileInfo.CompanyName,
                Product = _fileInfo.ProductName,
                Copyright = _fileInfo.LegalCopyright,
                Trademark = _fileInfo.LegalTrademarks,
                Culture = "",
                ComVisible = false,
                Version = System.Reflection.Assembly.LoadFile(File).GetName().Version,
                FileVersion = new Version(_fileInfo.FileVersion),
            };

            return _assemblyInfo;
        }

        /// <summary>
        /// Преобразует значение данного экземпляра в еквивалентное ему строковое представление.
        /// </summary>
        /// <param name="Language">Язык программирования</param>
        /// <returns></returns>
        public String ToString(ComputerLanguage Language)
        {
            List<String> Result;

            switch (Language)
            {
                case ComputerLanguage.CSharp:
                    Result = new List<String>
                    {
                        "using System.Reflection;",
                        "using System.Runtime.CompilerServices;",
                        "using System.Runtime.InteropServices;",
                        "",
                        "// Общие сведения об этой сборке предоставляются следующим набором набора атрибутов.",
                        "// Измените значения этих атрибутов, чтобы изменить сведения, связанные со сборкой.",
                        $"[assembly: AssemblyTitle(\"{this.Title}\")]",
                        $"[assembly: AssemblyDescription(\"{this.Description}\")]",
                        $"[assembly: AssemblyConfiguration(\"{this.Configuration}\")]",
                        $"[assembly: AssemblyCompany(\"{this.Company}\")]",
                        $"[assembly: AssemblyProduct(\"{this.Product}\")]",
                        $"[assembly: AssemblyCopyright(\"{this.Copyright}\")]",
                        $"[assembly: AssemblyTrademark(\"{this.Trademark}\")]",
                        $"[assembly: AssemblyCulture(\"{this.Culture}\")]",
                        "",
                        "// Установка значения False для параметра ComVisible делает типы в этой сборке невидимыми для компонентов COM.",
                        "// Если необходимо обратиться к типу в этой сборке через COM, задайте атрибуту ComVisible значение TRUE для этого типа.",
                        $"[assembly: ComVisible({this.ComVisible.ToString().ToLower()})]",
                        "",
                        "// Следующий GUID служит для идентификации библиотеки типов, если этот проект будет видимым для COM",
                        "// [assembly: Guid(\"\")]",
                        "",
                        "// Сведения о версии сборки состоят из следующих четырех значений:",
                        "//",
                        "//      Основной номер версии",
                        "//      Дополнительный номер версии",
                        "//      Номер сборки",
                        "//      Редакция",
                        "//",
                        "// Можно задать все значения или принять номер сборки и номер редакции по умолчанию, используя \"*\", как показано ниже:",
                        "// [assembly: AssemblyVersion(\"1.0.*\")]",
                        "",
                        "// Сведения о версии сборки.",
                        $"[assembly: AssemblyVersion(\"{this.Version.ToString()}\")]",
                        $"[assembly: AssemblyFileVersion(\"{this.FileVersion.ToString()}\")]"
                    };
                    break;

                case ComputerLanguage.VisualBasic:
                    Result = new List<String>
                    {
                        "Imports System",
                        "Imports System.Reflection",
                        "Imports System.Runtime.InteropServices",
                        "",
                        "' Общие сведения об этой сборке предоставляются следующим набором атрибутов.",
                        "' Измените значения этих атрибутов, чтобы изменить общие сведения об этой сборке.",
                        $"<Assembly: AssemblyTitle(\"{this.Title}\")>",
                        $"<Assembly: AssemblyDescription(\"{this.Description}\")>",
                        $"<assembly: AssemblyConfiguration(\"{this.Configuration}\")>",
                        $"<Assembly: AssemblyCompany(\"{this.Company}\")>",
                        $"<Assembly: AssemblyProduct(\"{this.Product}\")>",
                        $"<Assembly: AssemblyCopyright(\"{this.Copyright}\")>",
                        $"<Assembly: AssemblyTrademark(\"{this.Trademark}\")>",
                        $"<assembly: AssemblyCulture(\"{this.Culture}\")>",
                        "",
                        $"<Assembly: ComVisible(\"{this.ComVisible.ToString().ToLower()}\")>",
                        "",
                        "' Следующий GUID служит для идентификации библиотеки типов, если этот проект будет видимым для COM",
                        "' <Assembly: Guid(\"\")>",
                        "",
                        "' Сведения о версии сборки состоят из следующих четырех значений:",
                        "'",
                        "'      Основной номер версии",
                        "'      Дополнительный номер версии",
                        "'      Номер сборки",
                        "'      Редакция",
                        "'",
                        "' Можно задать все значения или принять номер сборки и номер редакции по умолчанию, используя \"*\", как показано ниже:",
                        "' <Assembly: AssemblyVersion(\"1.0.*\")>",
                        "",
                        $"<Assembly: AssemblyVersion(\"{this.Version.ToString()}\")>",
                        $"<Assembly: AssemblyFileVersion(\"{this.FileVersion.ToString()}\")>",
                    };
                    break;

                default:
                    Result = new List<String>
                    {
                        ""
                    };
                    break;
            }

            return Result.Aggregate("", (Content, Item) => Content += Item + Environment.NewLine);
        }

        /// <summary>
        /// Сохраняет данные в файл "AssemblyInfo.*".
        /// </summary>
        /// <param name="File">Расположение файла</param>
        /// <param name="Language">Язык программирования</param>
        public void Save(String File, ComputerLanguage Language)
        {
            using (FileStream FS = new FileStream(File, FileMode.Create))
            {
                Byte[] Buffer = new UTF8Encoding(true).GetBytes(ToString(Language));
                FS.Write(Buffer, 0, Buffer.Length);

                FS.Close();
                FS.Dispose();
            }
        }
    }
}
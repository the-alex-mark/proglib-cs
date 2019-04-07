using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Общие сведения об этой сборке.
    /// </summary>
    public class Property
    {
        public Property()
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

        public Property(String Title, String Description, String Configuration, String Company, String Product, String Copyright, String Trademark, String Culture, Boolean ComVisible, String Guid, Version Version, Version FileVersion)
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

        // Общие сведения об этой сборке предоставляются следующим набором
        // набора атрибутов. Измените значения этих атрибутов, чтобы изменить сведения, связанные со сборкой.
        public String Title { get; set; }
        public String Description { get; set; }
        public String Configuration { get; set; }
        public String Company { get; set; }
        public String Product { get; set; }
        public String Copyright { get; set; }
        public String Trademark { get; set; }
        public String Culture { get; set; }

        /// <summary>
        /// Видимостьтипов данной сборки для компонентов COM.
        /// </summary>
        public Boolean ComVisible { get; set; }

        /// <summary>
        /// Сведения о версии сборки.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Сведения о версии файла.
        /// </summary>
        public Version FileVersion { get; set; }

        /// <summary>
        /// Преобразует значение данного экземпляра в еквивалентное ему строковое представление.
        /// </summary>
        /// <returns></returns>
        public new String ToString()
        {
            List<String> Result = new List<String>
            {
                "using System.Reflection;",
                "using System.Runtime.CompilerServices;",
                "using System.Runtime.InteropServices;",
                "",
                "// Общие сведения об этой сборке.",
                $"[assembly: AssemblyTitle(\"{this.Title}\")]",
                $"[assembly: AssemblyDescription(\"{this.Description}\")]",
                $"[assembly: AssemblyConfiguration(\"{this.Configuration}\")]",
                $"[assembly: AssemblyCompany(\"{this.Company}\")]",
                $"[assembly: AssemblyProduct(\"{this.Product}\")]",
                $"[assembly: AssemblyCopyright(\"{this.Copyright}\")]",
                $"[assembly: AssemblyTrademark(\"{this.Trademark}\")]",
                $"[assembly: AssemblyCulture(\"{this.Culture}\")]",
                "",
                "// Видимостьтипов данной сборки для компонентов COM.",
                $"[assembly: ComVisible({this.ComVisible.ToString().ToLower()})]",
                "",
                "// Сведения о версии сборки.",
                $"[assembly: AssemblyVersion(\"{this.Version.ToString()}\")]",
                $"[assembly: AssemblyFileVersion(\"{this.FileVersion.ToString()}\")]"
            };

            return Result.Aggregate("", (Content, Item) => Content += Item + Environment.NewLine);
        }

        /// <summary>
        /// Сохраняет данные в файл "AssemblyInfo.cs"
        /// </summary>
        /// <param name="File"></param>
        public void Save(String File)
        {
            using (FileStream FS = new FileStream(File, FileMode.Create))
            {
                Byte[] Buffer = new UTF8Encoding(true).GetBytes(ToString());
                FS.Write(Buffer, 0, Buffer.Length);

                FS.Close();
                FS.Dispose();
            }
        }
    }
}

using ProgLib.Diagnostics;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Data
{
    public class Compiler
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Compiler"/>
        /// </summary>
        /// <param name="Language">Язык программирования, используемый для сборки проекта</param>
        public Compiler(CompilerLanguage Language)
        {
            // Установка языка программирования обрабатываемый компилятором
            this.Language = Language;

            // Установка параметров компилятора по умолчанию
            this.Parameters = new CompilerParameters
            {
                // Создаётся исполняемый файл вместо библиотеки классов.
                GenerateExecutable = true,
                
                // Создание отладочной информации.
                IncludeDebugInformation = false,

                // Сохранение сборки, как физический файл.
                GenerateInMemory = true,

                // Уровень, на котором компилятор должен начать отображать предупреждения.
                WarningLevel = 4,

                // Рассмотр всех предупреждений как ошибки.
                TreatWarningsAsErrors = false,

                // Аргумент компилятора для оптимизации вывода.
                // "/optimize"   - Включает (или отключает) оптимизацию.
                // "/platform"   - Пределы, какие платформы этот код можно запускать на: x86, Itanium, x64, AnyCPU или anycpu32bitpreferred. По умолчанию AnyCPU.
                // "/target"     - Задает формат выходного файла с помощью одного из четырех вариантов: /target:appcontainerexe, /target:exe, /target:library, /target:module, /target:winexe, /target:winmdobj.
                // "/unsafe"     - Позволяет небезопасный код.
                CompilerOptions = "/optimize"
            };
        }

        #region Properties

        /// <summary>
        /// Язык компилятора
        /// </summary>
        private CompilerLanguage Language
        {
            get;
            set;
        }

        /// <summary>
        /// Параметры, используемые для вызова компилятора
        /// </summary>
        public CompilerParameters Parameters
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Компилирует сборку из исходного кода, используя указанные параметры компилятора.
        /// </summary>
        /// <param name="Assembly">Исходный проект</param>
        public void Execute(Project Assembly)
        {

        }
    }

    public struct ProjectParameters
    {

    }

    /// <summary>
    /// Тип приложения
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// Библиотека классов (.NET Framework)
        /// </summary>
        Library,

        /// <summary>
        /// Консольное приложение (.NET Framework)
        /// </summary>
        Console,

        /// <summary>
        /// Приложение Windows Forms (.NET Framework)
        /// </summary>
        WindowsForms
    }

    /// <summary>
    /// Разрядность приложения
    /// </summary>
    public enum ProjectDigitCapacity
    {
        /// <summary>
        /// Подходит для всех разрядностей процессора
        /// </summary>
        AnyCPU,

        /// <summary>
        /// 32-битное расширение
        /// </summary>
        x86,

        /// <summary>
        /// 64-битное расширение
        /// </summary>
        x64
    }

    public class Project
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Project"/>
        /// </summary>
        /// <param name="Name">Имя сборки</param>
        /// <param name="Language">Язык программирования</param>
        public Project(String Name, CompilerLanguage Language)
        {
            this.Name = Name.Replace(new Char[] { ' ', '?', '!', '@', '`', '~', '$', '%', '^', ':', '&', '*', '(', ')', '=', '+', '<', '>', ',', '/', '\\', '\"', '«', '»', '—', '-', '№', '#' }, '_');
            this.Language = Language;

            // Установка списка связанных сборок по умолчанию
            ReferencedAssemblies = new List<String>()
            {
                "mscorlib.dll",
                "Microsoft.CSharp.dll",
                "System.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Data.DataSetExtensions.dll",
                "System.Deployment.dll",
                "System.Drawing.dll",
                "System.Net.Http.dll",
                "System.Windows.Forms.dll",
                "System.Xml.dll",
                "System.Xml.Linq.dll",
            };
        }

        #region Properties

        /// <summary>
        /// Имя проекта
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Язык компилятора
        /// </summary>
        public CompilerLanguage Language
        {
            get;
            private set;
        }

        /// <summary>
        /// Список связанных сборок
        /// </summary>
        public List<String> ReferencedAssemblies
        {
            get;
            set;
        }

        /// <summary>
        /// Настройки сборки проекта
        /// </summary>
        public ProjectParameters Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Общие сведения данного проекта
        /// </summary>
        public AssemblyInfo AssemblyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Список ресурсов
        /// </summary>
        public AssemblyResources Resources
        {
            get;
            set;
        }

        #endregion
    }
}

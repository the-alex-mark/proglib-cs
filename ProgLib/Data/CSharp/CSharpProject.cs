using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using ProgLib.Diagnostics;

namespace ProgLib.Data.CSharp
{
    // TODO: Завершить работу над компилятором "CSharpProject"

    /// <summary>
    /// Предоставляет функции, для работы с компилятором CSharp.
    /// </summary>
    public class CSharpProject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CSharpProject"/>.
        /// </summary>
        /// <param name="Name"></param>
        public CSharpProject(String Name)
        {
            this.AssemblyInfo = new AssemblyInfo();
            this.AssemblyInfo.Title = Name;

            this.Name = Name;
            this.Output = Environment.CurrentDirectory;
            this.Framework = "";
            this.Parameters = null;
            this.Resources = new CSharpProjectResources();

            Libraries = new List<String>()
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
        /// Общие сведения об этой сборке
        /// </summary>
        public AssemblyInfo AssemblyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Имя проекта
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Расположение выходной сборки
        /// </summary>
        public String Output
        {
            get;
            set;
        }

        /// <summary>
        /// Версия Framework
        /// </summary>
        public String Framework
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

        /// <summary>
        /// Список подключаемых библиотек
        /// </summary>
        public List<String> Libraries
        {
            get;
            set;
        }

        /// <summary>
        /// Список ресурсов
        /// </summary>
        public CSharpProjectResources Resources
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// создаёт временную папку.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private String CreateTempDirectory(String Path)
        {
            DirectoryInfo DI = Directory.CreateDirectory(Path);
            DI.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            return Path;
        }

        /// <summary>
        /// Удаляет указанную временную папку.
        /// </summary>
        /// <param name="Path"></param>
        private void DeleteTempDirectory(String Path)
        {
            if (Directory.Exists(Path))
                Directory.Delete(Path, true);
        }

        /// <summary>
        /// Удаляет указанную временную папку.
        /// </summary>
        /// <param name="Path"></param>
        private void DeleteAssembly(String Path)
        {
            if (Directory.Exists(Path))
                Directory.Delete(Path, true);
        }

        #endregion

        /// <summary>
        /// Компилирует сборку из исходного кода, содержащегося в указанных файлах, используя указанные параметры компилятора.
        /// </summary>
        /// <param name="Launch">Значение, указывающее будет ли исполняющий файл запущен после его компиляции</param>
        /// <param name="Files">Массив имён файлов для компиляции</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        public CSharpResult CompileAssemblyFromFiles(Boolean Launch, params String[] Files)
        {
            if (this.Name != "" && this.Output != "" && this.AssemblyInfo != null && this.Resources != null)
            {
                // Массив имён файлов для компиляции
                List<String> _files = Files.ToList();

                // Создание временной папки и дополнительных файлов
                String _tempDirectory = CreateTempDirectory(this.Output + @"\~Temp\");
                String _fileResources         = _tempDirectory + this.Name + ".Properties.Resources.resources";
                String _fileResourcesDesigner = _tempDirectory + this.Name + "Resources.Designer.cs";
                String _fileAssemblyInfo      = _tempDirectory + "AssemblyInfo.cs";

                #region Установка параметров компилятора

                // Создание файла c информацией о сборке
                this.AssemblyInfo.Save(_fileAssemblyInfo);
                _files.Add(_fileAssemblyInfo);

                // Предоставляет дотуп к экземплярам генератора кода C# и компилятора кода
                CSharpCodeProvider Provider = (this.Framework != "")
                    ? new CSharpCodeProvider(new Dictionary<String, String>() { { "CompilerVersion", this.Framework } })
                    : new CSharpCodeProvider();

                if (this.Parameters == null)
                {
                    // Параметры компилятора
                    this.Parameters = new CompilerParameters
                    {
                        // Создаётся исполняемый файл вместо библиотеки классов.
                        GenerateExecutable = true,

                        // Полное имя файла сборки.
                        OutputAssembly = Output + "\\" + Name + ".exe",

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

                if (this.Resources.Count > 0)
                {
                    // Определение файла ресурсов.
                    this.Resources.Save(_fileResources);

                    if (Provider.Supports(GeneratorSupport.Resources))
                    {
                        // Установка встроенного файла ресурсов сборки.
                        this.Parameters.EmbeddedResources.Add(_fileResources);
                    }

                    this.Resources.Save(this, _fileResourcesDesigner);
                    _files.Add(_fileResourcesDesigner);
                }

                // Подключение внутренних и внешних библиотек.
                foreach (String Library in this.Libraries)
                    this.Parameters.ReferencedAssemblies.Add(Library);

                #endregion

                #region Компиляция исходного кода

                CompilerResults Results;

                try
                {
                    // Компиляция кода
                    Results = Provider.CompileAssemblyFromFile(this.Parameters, _files.ToArray());
                }
                catch (Exception _exception)
                {
                    DeleteTempDirectory(_tempDirectory);
                    DeleteAssembly(this.Parameters.OutputAssembly);

                    throw _exception;
                }

                DeleteTempDirectory(_tempDirectory);

                if (Results.Errors.Count == 0 && Launch)
                    Process.Start(this.Parameters.OutputAssembly);

                // Возврат ошибок компилятора
                return new CSharpResult(Results);

                #endregion
            }
            else { return new CSharpResult("Не все параметры были определены!"); }
        }
    }
}

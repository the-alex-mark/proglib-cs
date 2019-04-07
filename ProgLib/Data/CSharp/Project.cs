using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Предоставляет функции, для работы с компилятором
    /// </summary>
    public class Project
    {
        public Project()
        {
            this.Name = "";
            this.OutputAssembly = "";
            this.Framework = "";
            this.Property = null;
            this.Parameters = null;
            this.Resources = new Resources();

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

        public Project(String Name)
        {
            this.Name = Name;
            this.OutputAssembly = Environment.CurrentDirectory + "\\" + Name + ".exe";
            this.Framework = "";
            this.Property = null;
            this.Parameters = null;
            this.Resources = new Resources();

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

        /// <summary>
        /// Имя проекта.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Имя выходной сборки
        /// </summary>
        public String OutputAssembly { get; set; }

        public String Framework { get; set; }

        /// <summary>
        /// Общие сведения об этой сборке.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// Параметры, используемые для вызова компилятора.
        /// </summary>
        public CompilerParameters Parameters { get; set; }

        /// <summary>
        /// Список подключаемых библиотек.
        /// </summary>
        public List<String> Libraries { get; set; }

        /// <summary>
        /// Список ресурсов.
        /// </summary>
        public Resources Resources { get; set; }

        /// <summary>
        /// Сборка проекта.
        /// </summary>
        /// <returns></returns>
        public CSharpResult CompileAssemblyFromFiles(Boolean Launch, params String[] Files)
        {
            if (this.Name != "" && this.OutputAssembly != "" && this.Property != null && this.Resources != null)
            {
                List<String> ListFiles = Files.ToList();
                String TempDirectory = Path.GetDirectoryName(this.Parameters.OutputAssembly) + @"\~Temp\";
                DirectoryInfo DI = Directory.CreateDirectory(TempDirectory);
                DI.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                // Предоставляет дотуп к экземплярам генератора кода C# и компилятора кода.
                CSharpCodeProvider Provider = (this.Framework != "")
                    ? new CSharpCodeProvider(new Dictionary<String, String>() { { "CompilerVersion", this.Framework } })
                    : new CSharpCodeProvider();
                //CSharpCodeProvider Provider = new CSharpCodeProvider();

                if (this.Parameters == null)
                {
                    // Параметры компилятора
                    this.Parameters = new CompilerParameters
                    {
                        // Создаётся исполняемый файл вместо библиотеки классов.
                        GenerateExecutable = true,

                        // Полное имя файла сборки.
                        OutputAssembly = OutputAssembly,

                        // Создание отладочной информации.
                        IncludeDebugInformation = false,

                        // Сохранение сборки, как физический файл.
                        GenerateInMemory = true,

                        // Уровень, на котором компилятор должен начать отображать предупреждения.
                        WarningLevel = 3,

                        // Рассмотр всех предупреждений как ошибки.
                        TreatWarningsAsErrors = false,

                        // Аргумент компилятора для оптимизации вывода.
                        // "/optimize"   - Включает ( или отключает) оптимизацию.
                        // "/platform"   - Пределы, какие платформы этот код можно запускать на: x86, Itanium, x64, AnyCPU или anycpu32bitpreferred. По умолчанию AnyCPU.
                        // "/target"     - Задает формат выходного файла с помощью одного из четырех вариантов: /target:appcontainerexe, /target:exe, /target:library, /target:module, /target:winexe, /target:winmdobj.
                        // "/unsafe"     - Позволяет небезопасный код.
                        CompilerOptions = "/optimize"
                    };
                }

                if (this.Resources.Count > 0)
                {
                    // Определение файла ресурсов.
                    using (ResourceWriter RW = new ResourceWriter(TempDirectory + this.Name + ".Properties.Resources.resources"))
                    {
                        foreach (Resource Resource in this.Resources.List)
                            RW.AddResource(Resource.Name, Resource.Value);

                        RW.Close();
                        RW.Dispose();
                    }

                    if (Provider.Supports(GeneratorSupport.Resources))
                    {
                        // Установка встроенного файла ресурсов сборки.
                        this.Parameters.EmbeddedResources.Add(TempDirectory + this.Name + ".Properties.Resources.resources");
                    }

                    this.Resources.Save(this, TempDirectory + "Resources.Designer.cs");
                    ListFiles.Add(TempDirectory + "Resources.Designer.cs");
                }

                if (this.Property != null)
                {
                    // Создание файла информации о сборке
                    this.Property.Save(TempDirectory + "AssemblyInfo.cs");
                    ListFiles.Add(TempDirectory + "AssemblyInfo.cs");
                }

                // Подключение внутренних и внешних библиотек.
                foreach (String Library in this.Libraries)
                    this.Parameters.ReferencedAssemblies.Add(Library);

                CompilerResults Results;

                try
                {
                    // Компиляция кода
                    Results = Provider.CompileAssemblyFromFile(this.Parameters, ListFiles.ToArray());
                }
                catch (Exception _exception)
                {
                    if (Directory.Exists(TempDirectory))
                        Directory.Delete(TempDirectory, true);

                    throw _exception;
                }

                if (Directory.Exists(TempDirectory))
                    Directory.Delete(TempDirectory, true);

                if (Results.Errors.Count == 0 && Launch)
                    Process.Start(this.Parameters.OutputAssembly);

                // Возврат ошибок компилятора
                return new CSharpResult(Results);
            }
            else { return new CSharpResult("Не все параметры были определены!"); }
        }
    }
}

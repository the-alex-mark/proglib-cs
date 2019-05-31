using ProgLib.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Список ресурсов.
    /// </summary>
    public class AssemblyResources
    {
        /// <summary>
        /// Инициализирует пустой экземпляр класса <see cref="AssemblyResources"/>.
        /// </summary>
        public AssemblyResources()
        {
            this.List = new Dictionary<String, Object>();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AssemblyResources"/>.
        /// </summary>
        /// <param name="Values"></param>
        public AssemblyResources(Dictionary<String, Object> Values)
        {
            this.List = Values;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AssemblyResources"/>.
        /// </summary>
        public static implicit operator AssemblyResources(Dictionary<String, Object> Values)
        {
            return new AssemblyResources(Values);
        }

        #region Properties
        
        /// <summary>
        /// Список ресурсов
        /// </summary>
        public Dictionary<String, Object> List
        {
            get;
            set;
        }

        /// <summary>
        /// Получает количество ресурсов
        /// </summary>
        public Int32 Count
        {
            get { return List.Count; }
        }

        #endregion

        /// <summary>
        /// Получает список ресурсов указанного именного файла ресурсов.
        /// </summary>
        /// <param name="File">Расположение файла</param>
        /// <returns></returns>
        public static AssemblyResources Load(String File)
        {
            Dictionary<String, Object> _list = new Dictionary<String, Object>();

            using (ResourceReader RR = new ResourceReader(File))
            {
                IDictionaryEnumerator _resources = RR.GetEnumerator();
                while (_resources.MoveNext())
                {
                    _list.Add(_resources.Key.ToString(), _resources.Value);
                }

                RR.Close();
                RR.Dispose();
            }

            return new AssemblyResources(_list);
        }

        /// <summary>
        /// Получает список ресурсов указанного именного файла ресурсов.
        /// </summary>
        /// <param name="Stream"></param>
        /// <returns></returns>
        public static AssemblyResources Load(FileStream Stream)
        {
            Dictionary<String, Object> _list = new Dictionary<String, Object>();

            using (ResourceReader RR = new ResourceReader(Stream))
            {
                IDictionaryEnumerator _resources = RR.GetEnumerator();
                while (_resources.MoveNext())
                {
                    _list.Add(_resources.Key.ToString(), _resources.Value);
                }

                RR.Close();
                RR.Dispose();
            }

            Stream.Close();
            Stream.Dispose();

            return new AssemblyResources(_list);
        }

        /// <summary>
        /// Добавляет новый ресурс.
        /// </summary>
        /// <param name="Name">Имя ресурса</param>
        /// <param name="Value">Значение ресурса</param>
        public void Add(String Name, Object Value)
        {
            List.Add(Name, Value);
        }
        
        /// <summary>
        /// Удаляет ресурс с указанным именем.
        /// </summary>
        /// <param name="Name"></param>
        public void Remove(String Name)
        {
            List.Remove(Name);
        }

        /// <summary>
        /// Удаляет все элементы из списка ресурсов.
        /// </summary>
        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        /// Преобразует значение данного экземпляра в еквивалентное ему строковое представление.
        /// </summary>
        /// <param name="Assembly">Имя сборки</param>
        /// <param name="Language">Язык программирования</param>
        /// <returns></returns>
        public String ToString(String Assembly, ComputerLanguage Language)
        {
            List<String> Result;

            switch (Language)
            {
                case ComputerLanguage.CSharp:
                    Result = new List<String>()
                    {
                        "namespace " + Assembly + ".Properties",
                        "{",
                        "    /// <summary>",
                        "    /// Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.",
                        "    /// </summary>",
                        "    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.Resources.Tools.StronglyTypedResourceBuilder\", \"4.0.0.0\")]",
                        "    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]",
                        "    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]",
                        "    internal class Resources",
                        "    {",
                        "        private static global::System.Resources.ResourceManager resourceMan;",
                        "        private static global::System.Globalization.CultureInfo resourceCulture;",
                        "",
                        "        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(\"Microsoft.Performance\", \"CA1811:AvoidUncalledPrivateCode\")]",
                        "        internal Resources() { }",
                        "",
                        "        /// <summary>",
                        "        /// Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.",
                        "        /// </summary>",
                        "        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]",
                        "        internal static global::System.Resources.ResourceManager ResourceManager",
                        "        {",
                        "            get",
                        "            {",
                        "                if (object.ReferenceEquals(resourceMan, null))",
                        "                {",
                        "                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(\"" + Assembly + ".Properties.Resources\", typeof(Resources).Assembly);",
                        "                    resourceMan = temp;",
                        "                }",
                        "                return resourceMan;",
                        "            }",
                        "        }",
                        "",
                        "        /// <summary>",
                        "        /// Перезаписывает свойство CurrentUICulture текущего потока для всех обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.",
                        "        /// </summary>",
                        "        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]",
                        "        internal static global::System.Globalization.CultureInfo Culture",
                        "        {",
                        "            get { return resourceCulture; }",
                        "            set { resourceCulture = value; }",
                        "        }"
                    };

                    foreach (KeyValuePair<String, Object> Resource in List)
                    {
                        Result.Add("");
                        Result.Add("        /// <summary>");
                        Result.Add("        /// Поиск локализованного ресурса типа " + Resource.Value.GetType().ToString() + ".");
                        Result.Add("        /// </summary>");
                        Result.Add("        internal static " + Resource.Value.GetType().ToString() + " " + Resource.Key + " {");
                        Result.Add("            get {");
                        Result.Add("                object obj = ResourceManager.GetObject(\"" + Resource.Key + "\", resourceCulture);");
                        Result.Add("                return ((" + Resource.Value.GetType().ToString() + ")(obj));");
                        Result.Add("            }");
                        Result.Add("        }");
                    }

                    Result.Add("    }");
                    Result.Add("}");
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
        /// Сохраняет данные в файл "*.Properties.Resources.resources".
        /// </summary>
        /// <param name="File">Расположение файла</param>
        public void Save(String File)
        {
            // Определение файла ресурсов.
            using (ResourceWriter RW = new ResourceWriter(File))
            {
                foreach (KeyValuePair<String, Object> Resource in List)
                    RW.AddResource(Resource.Key, Resource.Value);

                RW.Close();
                RW.Dispose();
            }
        }

        /// <summary>
        /// Сохраняет данные в файл "Resources.Designer.*".
        /// </summary>
        /// <param name="Assembly">Имя сборки</param>
        /// <param name="File">Расположение файла</param>
        /// <param name="Language">Язык программирования</param>
        public void Save(String Assembly, String File, ComputerLanguage Language)
        {
            using (FileStream FS = new FileStream(File, FileMode.Create))
            {
                Byte[] Buffer = new UTF8Encoding(true).GetBytes(ToString(Assembly, Language));
                FS.Write(Buffer, 0, Buffer.Length);

                FS.Close();
                FS.Dispose();
            }
        }
    }
}

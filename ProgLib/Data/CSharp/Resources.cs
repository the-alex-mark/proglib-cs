using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Список ресурсов.
    /// </summary>
    public class Resources
    {
        public Resources()
        {
            this.List = new List<Resource>();
        }

        public Resources(params Resource[] Values)
        {
            this.List = Values.Cast<Resource>().ToList();
        }

        public static implicit operator Resources(Resource[] Values)
        {
            return new Resources(Values);
        }

        /// <summary>
        /// Список ресурсов.
        /// </summary>
        public List<Resource> List { get; set; }

        /// <summary>
        /// Получает количество ресурсов
        /// </summary>
        public Int32 Count { get { return List.Count; } }

        /// <summary>
        /// Добавляет новый ресурс.
        /// </summary>
        /// <param name="Resource"></param>
        public void Add(Resource Resource)
        {
            List.Add(Resource);
        }

        /// <summary>
        /// Удаляет ресурс, по указанному индексу.
        /// </summary>
        /// <param name="Index"></param>
        public void Remove(Int32 Index)
        {
            List.RemoveAt(Index);
        }

        /// <summary>
        /// Полностью очищает список ресурсов.
        /// </summary>
        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        /// Преобразует значение данного экземпляра в еквивалентное ему строковое представление.
        /// </summary>
        /// <returns></returns>
        public String ToString(Project Project)
        {
            List<String> Result = new List<String>()
            {
                "namespace " + Project.Name + ".Properties",
                "{",
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
                "        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]",
                "        internal static global::System.Resources.ResourceManager ResourceManager",
                "        {",
                "            get",
                "            {",
                "                if (object.ReferenceEquals(resourceMan, null))",
                "                {",
                "                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(\"" + Project.Name + ".Properties.Resources\", typeof(Resources).Assembly);",
                "                    resourceMan = temp;",
                "                }",
                "                return resourceMan;",
                "            }",
                "        }",
                "",
                "        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]",
                "        internal static global::System.Globalization.CultureInfo Culture",
                "        {",
                "            get { return resourceCulture; }",
                "            set { resourceCulture = value; }",
                "        }"
            };

            foreach (Resource Resource in List)
            {
                Result.Add("");
                Result.Add("        internal static " + Resource.Value.GetType().ToString() + " " + Resource.Name + " {");
                Result.Add("            get {");
                Result.Add("                object obj = ResourceManager.GetObject(\"" + Resource.Name + "\", resourceCulture);");
                Result.Add("                return ((" + Resource.Value.GetType().ToString() + ")(obj));");
                Result.Add("            }");
                Result.Add("        }");
            }

            Result.Add("    }");
            Result.Add("}");

            return Result.Aggregate("", (Content, Item) => Content += Item + Environment.NewLine);
        }

        /// <summary>
        /// Сохраняет данные в файл "*.Properties.Resources.resources"
        /// </summary>
        /// <param name="Project"></param>
        /// <param name="File"></param>
        public void Save(Project Project, String File)
        {
            using (FileStream FS = new FileStream(File, FileMode.Create))
            {
                Byte[] Buffer = new UTF8Encoding(true).GetBytes(ToString(Project));
                FS.Write(Buffer, 0, Buffer.Length);

                FS.Close();
                FS.Dispose();
            }
        }
    }
}

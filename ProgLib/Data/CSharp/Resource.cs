using System;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Ресурс.
    /// </summary>
    public class Resource
    {
        public Resource()
        {
            this.Name = "";
            this.Value = "";
        }

        public Resource(String Name, Object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        /// <summary>
        /// Имя ресурса.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Значение ресурса.
        /// </summary>
        public Object Value { get; set; }
    }
}

using System;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Ресурс.
    /// </summary>
    public class CSharpProjectResource
    {
        /// <summary>
        /// Инициализирует экземпляр типа <see cref="CSharpProjectResource"/>.
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Value">Ресурс</param>
        public CSharpProjectResource(String Name, Object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        #region Properties

        /// <summary>
        /// Имя.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Ресурс.
        /// </summary>
        public Object Value { get; set; }

        #endregion
    }
}

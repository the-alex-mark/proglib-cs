using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет структуру для хранения информации об установленном программной обеспечении
    /// </summary>
    public struct Software
    {
        /// <summary>
        /// Название
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public DateTime InstallDate { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public String Version { get; set; }
    }
}

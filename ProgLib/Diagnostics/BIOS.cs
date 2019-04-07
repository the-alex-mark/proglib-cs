using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет структуру для хранения информации об установленном BIOS'е
    /// </summary>
    public struct BIOS
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
        /// Производитель
        /// </summary>
        public String Manufacturer { get; set; }

        /// <summary>
        /// Серийный номер
        /// </summary>
        public String SerialNumber { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public String Version { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public String BIOSVersion { get; set; }

        // Другие версии
        public String SMBIOSBIOSVersion { get; set; }
        public Int32 SMBIOSMajorVersion { get; set; }
        public Int32 SMBIOSMinorVersion { get; set; }
        public Int32 SystemBiosMajorVersion { get; set; }
        public Int32 SystemBiosMinorVersion { get; set; }
    }
}

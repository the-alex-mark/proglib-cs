using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет структуру для хранения информации об операционной системе
    /// </summary>
    public struct OperatingSystem
    {
        /// <summary>
        /// Название
        /// </summary>
        public String Caption { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Каталог
        /// </summary>
        public String Directory { get; set; }

        /// <summary>
        /// Системный диск
        /// </summary>
        public String Drive { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public String Manufacturer { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public String Version { get; set; }

        /// <summary>
        /// Серийный номер
        /// </summary>
        public String SerialNumber { get; set; }

        /// <summary>
        /// Разрядность
        /// </summary>
        public String Bit { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public DateTime InstallDate { get; set; }

        /// <summary>
        /// Максимальное количество процессов
        /// </summary>
        public Int32 MaxNumberOfProcesses { get; set; }

        /// <summary>
        /// Максимальный размер памяти процесса
        /// </summary>
        public Int32 MaxProcessMemorySize { get; set; }
    }
}

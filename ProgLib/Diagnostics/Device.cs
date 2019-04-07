using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет базовый класс для хранения информации о доступных устройствах
    /// </summary>
    public class Device
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
    }
}

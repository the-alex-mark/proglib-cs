using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Предоставляет структуру для хранения информации о досупных приводах
    /// </summary>
    public class Drive : Device
    {
        /// <summary>
        /// Тип
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// Метка тома
        /// </summary>
        public String LogicalName { get; set; }
        
        /// <summary>
        /// Метка тома
        /// </summary>
        public String VolumeLabel { get; set; }

        /// <summary>
        /// Файловая система
        /// </summary>
        public String DriveFormat { get; set; }

        /// <summary>
        /// Доступное пространство для текущего пользователя
        /// </summary>
        public Int64 AvailableFreeSpace { get; set; }

        /// <summary>
        /// Общая свободная площадь
        /// </summary>
        public Int64 TotalFreeSpace { get; set; }

        /// <summary>
        /// Общий размер привода
        /// </summary>
        public Int64 TotalSize { get; set; }
    }
}

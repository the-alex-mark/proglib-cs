using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Процессор
    /// </summary>
    public class Processor : Device
    {
        /// <summary>
        /// Число ядер
        /// </summary>
        public Int32 NumberOfCores { get; set; }

        /// <summary>
        /// Тактовая частота (ГГц)
        /// </summary>
        public Double CurrentClockSpeed { get; set; }
    }
}

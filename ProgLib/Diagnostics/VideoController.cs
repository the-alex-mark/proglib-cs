using System;

namespace ProgLib.Diagnostics
{
    /// <summary>
    /// Видеокарта
    /// </summary>
    public class VideoController : Device
    {
        /// <summary>
        /// Видеорежим
        /// </summary>
        public String VideoModeDescription { get; set; }
    }
}

using System;

namespace ProgLib.Text
{
    /// <summary>
    /// Список форматов преобразования текста.
    /// </summary>
    public enum TranslitType
    {
        /// <summary>
        /// ГОСТ 16876-71
        /// </summary>
        GOST,

        /// <summary>
        /// ISO 9-95
        /// </summary>
        ISO,

        /// <summary>
        /// Пользовательский тип преобразования
        /// </summary>
        Custom
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Windows.Forms.Cyotek
{
    /// <summary>
    /// Задает порядок сортировки цветов
    /// </summary>
    public enum ColorCollectionSortOrder
    {
        /// <summary>
        /// Ordered by hue.
        /// </summary>
        Hue,

        /// <summary>
        /// Ordered by brightness.
        /// </summary>
        Brightness,

        /// <summary>
        /// Ordered by value
        /// </summary>
        Value
    }
}

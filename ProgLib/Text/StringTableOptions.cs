using System;
using System.Collections.Generic;

namespace ProgLib.Text
{
    /// <summary>
    /// Параметры таблицы
    /// </summary>
    public class StringTableOptions
    {
        public IEnumerable<String> Columns { get; set; } = new List<String>();
        public Boolean EnableCount { get; set; } = true;
    }
}

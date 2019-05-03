using System;
using System.IO;

namespace ProgLib.Plugins
{
    public class Bass
    {
        /// <summary>
        /// Сохраняет требующиеся библиотеки рядом с основной.
        /// </summary>
        public static void Start()
        {
            File.WriteAllBytes("bass.dll", Properties.Resources.bass);
            File.WriteAllBytes("basswasapi.dll", Properties.Resources.basswasapi);
        }
    }
}

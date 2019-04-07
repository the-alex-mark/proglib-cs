using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ProgLib.Text
{
    public class FontFromResources
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, UInt32 cbFont, IntPtr pdv, [In] ref UInt32 pcFonts);

        /// <summary>
        /// Возвращает объект класса Font с пользовательским шрифтом
        /// </summary>
        /// <param name="Resource"></param>
        /// <returns></returns>
        public static Font UseFont(Byte[] Resource)
        {
            PrivateFontCollection _fontCollection = new PrivateFontCollection();

            // Создание небезопасного блока памяти для данного шрифта
            IntPtr _memory = Marshal.AllocCoTaskMem(Resource.Length);

            // Копирование байтов в небезопасный блок памяти
            Marshal.Copy(Resource, 0, _memory, Resource.Length);

            // Передача шрифта в коллекцию
            _fontCollection.AddMemoryFont(_memory, Resource.Length);

            UInt32 _dummy = 0;
            AddFontMemResourceEx(_memory, (uint)Resource.Length, IntPtr.Zero, ref _dummy);

            // Освобождение небезопасной памяти
            Marshal.FreeCoTaskMem(_memory);

            return new Font(_fontCollection.Families[0], 8.25F);
        }
    }
}

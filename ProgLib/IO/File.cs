using System;
using System.IO;
using System.Text;

namespace ProgLib.IO
{
    public class File
    {
        /// <summary>
        /// Создаёт ярклык для указанного файла.
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Path"></param>
        public static void Shortcut(String File, String Path)
        {
            ShortCut.ShortCut.Create(File, Path, "", "");
        }
        
        /// <summary>
        /// Удаляет атрибут "Hidden" указанного файла.
        /// </summary>
        /// <param name="File"></param>
        public static void RemoveAttribute(String File)
        {
            FileAttributes Attributes = System.IO.File.GetAttributes(File);
            Attributes = RemoveAttribute(Attributes, FileAttributes.Hidden);
            System.IO.File.SetAttributes(File, Attributes);
        }
        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        /// <summary>
        /// Устанавливает выбранный атрибут указанному файлу.
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Attribute"></param>
        public static void Attribute(String File, FileAttributes Attribute)
        {
            switch (Attribute)
            {
                case FileAttributes.Normal:
                    System.IO.File.SetAttributes(File, System.IO.File.GetAttributes(File) | FileAttributes.Normal);
                    break;
                case FileAttributes.Hidden:
                    System.IO.File.SetAttributes(File, System.IO.File.GetAttributes(File) | FileAttributes.Hidden);
                    break;
                case FileAttributes.System:
                    System.IO.File.SetAttributes(File, System.IO.File.GetAttributes(File) | FileAttributes.System);
                    break;
                case FileAttributes.ReadOnly:
                    System.IO.File.SetAttributes(File, System.IO.File.GetAttributes(File) | FileAttributes.ReadOnly);
                    break;

                default: break;
            }
        }

        /// <summary>
        /// Получает MD5 указанного файла.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static String MD5(String File)
        {
            System.Security.Cryptography.MD5 MD5Hasher = System.Security.Cryptography.MD5.Create();
            Byte[] Data = MD5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(File));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < Data.Length; i++) { sBuilder.Append(Data[i].ToString("x2")); }
            return sBuilder.ToString();
        }
    }
}

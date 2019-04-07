using System;
using System.Collections.Generic;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;

namespace ProgLib.IO
{
    /// <summary>
    /// осуществляет работу над архивами формата ".zip"
    /// </summary>
    public class Archive
    {
        /// <summary>
        /// Получает список файлов содержащихся в архиве.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static String[] GetFiles(String Path)
        {
            List<String> Files = new List<String>();
            using (ZipFile zip = ZipFile.Read(Path))
            {
                foreach (ZipEntry ze in zip)
                {
                    if (ze.FileName.LastIndexOf('/') != ze.FileName.Length - 1)
                        Files.Add(ze.FileName);
                }
            }

            return Files.ToArray();
        }

        /// <summary>
        /// Создаёт архив.
        /// </summary>
        /// <param name="Path"></param>
        public static void Create(String Path)
        {
            ZipFile Archive = new ZipFile();
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Создаёт архив
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Comment"></param>
        public static void Create(String Path, String Comment)
        {
            ZipFile Archive = new ZipFile
            {
                Comment = Comment
            };
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Создаёт защищённый архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Comment"></param>
        /// <param name="Password"></param>
        public static void Create(String Path, String Password, String Comment)
        {
            ZipFile Archive = new ZipFile
            {
                Password = Password,
                Comment = Comment
            };
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добалвяет файл в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="File"></param>
        public static void AddFile(String Path, String File)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddFile(File, "");
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добалвяет файл в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="File"></param>
        /// <param name="DirectoryPathInArchive"></param>
        public static void AddFile(String Path, String File, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddFile(File, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добавляет файл в защищённый архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="DirectoryPathInArchive"></param>
        /// <param name="File"></param>
        public static void AddFile(String Path, String Password, String File, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression,
                Password = Password
            };
            Archive.AddFile(File, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добалвяет файлы в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Files"></param>
        public static void AddFiles(String Path, String[] Files)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddFiles(Files, "");
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добалвяет файлы в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Files"></param>
        /// <param name="DirectoryPathInArchive"></param>
        public static void AddFiles(String Path, String[] Files, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddFiles(Files, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добавляет файлы в защищённый архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="DirectoryPathInArchive"></param>
        /// <param name="Files"></param>
        public static void AddFiles(String Path, String Password, String[] Files, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression,
                Password = Password
            };
            Archive.AddFiles(Files, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добавляет папку в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Directory"></param>
        public static void AddDirectory(String Path, String Directory)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddDirectory(Directory, "");
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добавляет папку в архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Directory"></param>
        /// <param name="DirectoryPathInArchive"></param>
        public static void AddDirectory(String Path, String Directory, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression
            };
            Archive.AddDirectory(Directory, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Добавляет папку в защищённый архив.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Password"></param>
        /// <param name="Directory"></param>
        /// <param name="DirectoryPathInArchive"></param>
        public static void AddDirectory(String Path, String Password, String Directory, String DirectoryPathInArchive)
        {
            ZipFile Archive = new ZipFile(Path)
            {
                CompressionLevel = CompressionLevel.BestCompression,
                Password = Password
            };
            Archive.AddDirectory(Directory, DirectoryPathInArchive);
            Archive.Save(Path);

            Archive.Dispose();
        }

        /// <summary>
        /// Удаляет файл из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="File"></param>
        public static void RemoveFile(String Path, String File)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                Archive.RemoveEntry(File);
                Archive.Save();
            }
        }

        /// <summary>
        /// Удаляет файлы из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Files"></param>
        public static void RemoveFiles(String Path, String[] Files)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                Archive.RemoveEntries(Files.ToList());
                Archive.Save();

                Archive.Dispose();
            }
        }

        /// <summary>
        /// Удаляет папку из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Directory"></param>
        public static void RemoveDirectory(String Path, String Directory)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                Archive.RemoveSelectedEntries(Directory + "/*");
                Archive.Save();

                Archive.Dispose();
            }
        }

        /// <summary>
        /// Извлекает файл из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="File"></param>
        /// <param name="ExtractPath"></param>
        public static void Extract(String Path, String File, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                {
                    if (e.FileName == File)
                        e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
                }
            }
        }

        /// <summary>
        /// Извлекает файл из защищённого архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Password"></param>
        /// <param name="File"></param>
        /// <param name="ExtractPath"></param>
        public static void Extract(String Path, String Password, String File, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                {
                    if (e.FileName == File)
                    {
                        e.Password = Password;
                        e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
                    }
                }
            }
        }

        /// <summary>
        /// Извлекает файлы из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Files"></param>
        /// <param name="ExtractPath"></param>
        public static void Extract(String Path, String[] Files, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                {
                    foreach (String File in Files)
                    {
                        if (e.FileName == File)
                            e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
                    }
                }
            }
        }

        /// <summary>
        /// Извлекает файлы из защищённого архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Password"></param>
        /// <param name="Files"></param>
        /// <param name="ExtractPath"></param>
        public static void Extract(String Path, String Password, String[] Files, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                {
                    foreach (String File in Files)
                    {
                        if (e.FileName == File)
                        {
                            e.Password = Password;
                            e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Извлекает все файлы из архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="ExtractPath"></param>
        public static void ExtractAll(String Path, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                    e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
            }
        }

        /// <summary>
        /// Извлекает все файлы из защищённого архива.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Password"></param>
        /// <param name="ExtractPath"></param>
        public static void ExtractAll(String Path, String Password, String ExtractPath)
        {
            using (ZipFile Archive = ZipFile.Read(Path))
            {
                foreach (ZipEntry e in Archive.Entries)
                {
                    e.Password = Password;
                    e.Extract(ExtractPath, ExtractExistingFileAction.DoNotOverwrite);
                }
            }
        }
    }
}

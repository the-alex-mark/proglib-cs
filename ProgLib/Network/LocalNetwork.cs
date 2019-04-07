using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace ProgLib.Network
{
    public sealed class LocalNetwork
    {
        #region Dll Imports

        // Объявление Netapi32: Импорт метода NetServerEnum
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]

        /// <summary>
        /// Netapi32.dll : Функция NetServerEnum перечисляет все серверы указанного типа, видимые в домене.
        /// Например, приложение может вызвать NetServerEnum, чтобы вывести список только всех контроллеров домена или только всех серверов SQL.
        ///	Битовые маски можно комбинировать для отображения нескольких типов. Например, значение 0x00000003 объединяет битовые маски для SV_TYPE_WORKSTATION (0x00000001) и SV_TYPE_SERVER (0x00000002)
        /// </summary>
        public static extern int NetServerEnum(
            string ServerNane, // Должно быть null
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain, // null для входа в домен
            out int dwResumeHandle
            );

        // Объявление Netapi32: Импорт метода NetApiBufferFree
        [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]

        /// <summary>
        /// Netapi32.dll : Функция NetApiBufferFree освобождает память, выделяемую функцией NetApiBufferAllocate.
        /// Вызовите NetApiBufferFree, чтобы освободить память, возвращаемую другими функциями сетевого управления.
        /// </summary>
        public static extern int NetApiBufferFree(IntPtr pBuf);

        // Создание структуры _SERVER_INFO_100
        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal Int32 sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal String sv100_name;
        }

        #endregion

        public LocalNetwork()
        {

        }

        // Документация: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
        /// <summary>
        /// Возващает список имён компьютеров, ноходящихся внутри локальной сети
        /// </summary>
        /// <param name="Method">Тип метода сбора информации (от 1 до 2)</param>
        /// <returns></returns>
        public String[] GetListMachines(Int32 Method)
        {
            switch (Method)
            {
                case 1:
                    List<String> Computers = new List<String>();

                    DirectoryEntry Root = new DirectoryEntry("WinNT:");
                    foreach (DirectoryEntry AvailDomains in Root.Children)
                    {
                        foreach (DirectoryEntry PCNameEntry in AvailDomains.Children)
                        {
                            if (PCNameEntry.SchemaClassName.ToLower().Contains("computer"))
                                Computers.Add(PCNameEntry.Name);
                        }
                    }

                    return Computers.ToArray();

                case 2:
                    // Локальные поля
                    ArrayList networkComputers = new ArrayList();
                    const Int32 MAX_PREFERRED_LENGTH = -1;
                    Int32 SV_TYPE_WORKSTATION = 1;
                    Int32 SV_TYPE_SERVER = 2;
                    IntPtr buffer = IntPtr.Zero;
                    IntPtr tmpBuffer = IntPtr.Zero;
                    Int32 entriesRead = 0;
                    Int32 totalEntries = 0;
                    Int32 resHandle = 0;
                    Int32 sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));

                    try
                    {
                        // Вызов импортированного метода из Netapi32
                        Int32 ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out resHandle);

                        // Если возвращено с ERROR_Success (термин C++), =0 для C#
                        if (ret == 0)
                        {
                            // Цикл через все SV_TYPE_WORKSTATION и SV_TYPE_SERVER
                            for (int i = 0; i < totalEntries; i++)
                            {
                                tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));

                                _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)
                                Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

                                // Добавление имён ПК в массив
                                networkComputers.Add(svrInfo.sv100_name);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Проблема с доступом к сетевым компьютерам в сетевом браузере " + "\r\n\r\n\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    finally
                    {
                        // Функция NetApiBufferFree освобождает память, выделяемую функцией NetApiBufferAllocate
                        NetApiBufferFree(buffer);
                    }

                    return networkComputers.ToArray(typeof(String)) as String[];

                default:
                    throw new Exception("Входной параметр \"Method\" имел неверное значение");
            }
        }
    }
}

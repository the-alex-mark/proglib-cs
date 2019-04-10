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
        #region Imports

        // Объявление Netapi32: Импорт метода NetServerEnum.
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]

        /// <summary>
        /// Перечисляет все серверы указанного типа, видимые в домене.
        /// </summary>
        private static extern NERR NetServerEnum(
            [MarshalAs(UnmanagedType.LPWStr)]string ServerName, // Должно быть null
            int Level, out IntPtr BufPtr,
            int PrefMaxLen, ref int EntriesRead,
            ref int TotalEntries, ServerTypes ServerType,
            [MarshalAs(UnmanagedType.LPWStr)] string Domain, // null для входа в домен
            int ResumeHandle);

        // Импорт метода NetApiBufferFree.
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]

        /// <summary>
        /// Функция NetApiBufferFree освобождает память, выделяемую функцией NetApiBufferAllocate.
        /// </summary>
        private static extern NERR NetApiBufferFree(IntPtr Buffer);

        [StructLayout(LayoutKind.Sequential)]
        private struct ServerInfo
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint PlatformID;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string Name;

            [MarshalAs(UnmanagedType.U4)]
            public uint VersionMajor;

            [MarshalAs(UnmanagedType.U4)]
            public uint VersionMinor;

            [MarshalAs(UnmanagedType.U4)]
            public uint Type;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string Comment;
        }

        /// <summary>
        /// Список ошибок, возвращаемых NetServerEnum.
        /// </summary>
        private enum NERR
        {
            NERR_Success = 0, // Успех
            ERROR_ACCESS_DENIED = 5,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_BAD_NETPATH = 53,
            ERROR_NETWORK_BUSY = 54,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_LEVEL = 124,
            ERROR_MORE_DATA = 234,
            ERROR_EXTENDED_ERROR = 1208,
            ERROR_NO_NETWORK = 1222,
            ERROR_INVALID_HANDLE_STATE = 1609,
            ERROR_NO_BROWSER_SERVERS_FOUND = 6118,
        }

        #endregion

        /// <summary>
        /// Получает список компьютеров в локальной сети.
        /// </summary>
        /// <returns></returns>
        public String[] GetMachinesList()
        {
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
        }

        /// <summary>
        /// Получает список серверов в локальной сети.
        /// </summary>
        /// <param name="Type">Тип сервера</param>
        /// <returns></returns>
        public String[] GetServerList(ServerTypes Type)
        {
            ServerInfo Info;
            IntPtr pInfo = IntPtr.Zero;
            Int32 etriesread = 0, totalentries = 0;
            List<String> Servers = new List<String>();

            try
            {
                NERR Errors = NetServerEnum(null, 101, out pInfo, -1, ref etriesread, ref totalentries, Type, null, 0);
                if ((Errors == NERR.NERR_Success || Errors == NERR.ERROR_MORE_DATA) && pInfo != IntPtr.Zero)
                {
                    Int32 ptr = pInfo.ToInt32();
                    for (int i = 0; i < etriesread; i++)
                    {
                        Info = (ServerInfo)Marshal.PtrToStructure(new IntPtr(ptr), typeof(ServerInfo));
                        Servers.Add(Info.Name.ToString());

                        ptr += Marshal.SizeOf(Info);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                // Освобождение выделенной памяти
                if (pInfo != IntPtr.Zero) NetApiBufferFree(pInfo);
            }
            return Servers.ToArray();
        }
    }
}
